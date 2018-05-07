using AirPollutionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirPollutionApi.Controllers
{
    public class HomeController : ApiController
    {

        //from the users location lat, long, radius this api will give the stations within that radius range.
        [Route("api/HomePage/{latitude}/{longitude}/{radius}")]
        [HttpGet]
        public IHttpActionResult HomePage(string latitude, string longitude, string radius)
        {


            List<StationModel> homePageStationList = new List<StationModel>();
            StationModel sm = new StationModel();


            try
            {
                homePageStationList = sm.HomePage(latitude, longitude, radius);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return Ok(homePageStationList);
        }


        


        //this api gets string in input and based on that returns the similar city,state,country names
        [Route("api/SearchList/{loc}")]
        [HttpGet]
        public IHttpActionResult SearchList(string loc)
        {


          
            SearchModel objSearchList = new SearchModel();


            try
            {
                objSearchList = objSearchList.SearchList(loc);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return Ok(objSearchList);
        }



    }
}
