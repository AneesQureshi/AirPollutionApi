using AirPollutionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirPollutionApi.Controllers
{
    public class PollutantController : ApiController
    {

        //we get the station id and pollutant name  then give back the 
        //max polllutant value upto 7 days for that station id each day
      
        [Route("api/PollutantHistory/{stationId}/{pollutantName}")]
        [HttpGet]
        public IHttpActionResult PollutantHistory(string stationId,string pollutantName)
        {


            List<PollutantModel> pollutantHistoryList = new List<PollutantModel>();
            PollutantModel pm = new PollutantModel();


            try
            {
                pollutantHistoryList = pm.PollutantHistory(stationId,pollutantName);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return Ok(pollutantHistoryList);
        }
    }
}
