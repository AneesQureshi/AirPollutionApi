using AirPollutionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirPollutionApi.Controllers
{
    public class StationController : ApiController
    {

        //we get the station id and give back the 
        //aqi value of that station, station name
        //pollutant name ,pollutant value
        //date, aqi max value

        [Route("api/StationPage/{stationId}")]
        [HttpGet]
        public IHttpActionResult StationPage(string stationId)
        {


            StationHomePageModel StationHomePage = new StationHomePageModel();
            StationModel sm = new StationModel();


            try
            {
                StationHomePage = sm.stationHomePage(stationId);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return Ok(StationHomePage);
        }
    }
}
