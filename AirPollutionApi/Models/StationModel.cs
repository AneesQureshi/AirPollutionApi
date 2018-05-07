using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class StationModel
    {
        public string stationId { get; set; }
        public string stationName { get; set; }
        public string aqi { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string lastUpdatedDate { get; set; }


        //from the users location lat, long, radius this api will give the stations within that radius range.
        public List<StationModel> HomePage(string latitude, string longitude, string radius)
        {

            List<StationModel> homePageStationList = new List<StationModel>();

            try
            {

                DbHelper db = new DbHelper();
                homePageStationList = db.HomePage(latitude, longitude, radius);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return homePageStationList;

        }

        

        //we get the station id and give back the 
        //aqi value of that station, station name
        //pollutant name ,pollutant value
        //date, aqi max value

        public StationHomePageModel stationHomePage(string stationId)
        {

            StationHomePageModel StationHomePage = new StationHomePageModel();

            try
            {

                DbHelper db = new DbHelper();
                StationHomePage = db.stationHomePage(stationId);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return StationHomePage;

        }

    }
}