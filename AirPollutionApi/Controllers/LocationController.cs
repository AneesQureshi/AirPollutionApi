using AirPollutionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirPollutionApi.Controllers
{
    public class LocationController : ApiController
    {
        //if user selects city then this id will give list of all stations, aqi value, lat,long to plot in graph
        //else if user select station then this api will give the data of station and their pollutant

        [Route("api/SearchLocation/{id}/{location}")]
        [HttpGet]
        public IHttpActionResult SearchLocation(string id, string location)
        {


            List<StationModel> StationInCityList = new List<StationModel>();
            LocationModel lm = new LocationModel();



            try
            {
                if (location == "city")
                {
                    StationInCityList = lm.StationInCity(id);
                    return Ok(StationInCityList);
                }
               


            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return Ok(StationInCityList);
        }
    }
}
