using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class PollutantModel
    {
        public string pollutantName { get; set; }
        public string pollutantValue { get; set; }
        public string lastUpdatedDate { get; set; }

        //we get the station id and pollutant name  then give back the 
        //max polllutant value upto 7 days for that station id each day
        public List<PollutantModel> PollutantHistory(string stationId, string pollutantName)
        {

            List<PollutantModel> pollutantHistoryList = new List<PollutantModel>();

            try
            {

                DbHelper db = new DbHelper();
                pollutantHistoryList = db.PollutantHistory(stationId, pollutantName);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return pollutantHistoryList;

        }

    }

}