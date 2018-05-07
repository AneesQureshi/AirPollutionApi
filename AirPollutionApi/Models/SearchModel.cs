using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class SearchModel
    {
        public List<StationModel> stationList { get; set; }
        public List<LocationModel> cityList { get; set; }

        //this api gets string in input and based on that returns the similar station names &  city,state,country names
        public SearchModel SearchList(string loc)
        {

            SearchModel objSearchList = new SearchModel();

            try
            {

                DbHelper db = new DbHelper();
                objSearchList = db.SearchList(loc);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return objSearchList;
        }
    }
}