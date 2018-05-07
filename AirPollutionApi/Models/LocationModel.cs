using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class LocationModel
    {
      
       
       
        public string cityId { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }

       


        //if user selects city then this id will give list of all stations, aqi value, lat,long to plot in graph
        public List<StationModel> StationInCity(string id)
        {

            List<StationModel> StationInCityList = new List<StationModel>();

            try
            {

                DbHelper db = new DbHelper();
                StationInCityList = db.StationInCity(id);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return StationInCityList;

        }
    }
}