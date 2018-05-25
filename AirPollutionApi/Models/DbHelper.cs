using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class DbHelper
    {

        DbConnect db = new DbConnect();
        MySqlConnection con;
        MySqlCommand cmd;

        ////from the users location lat, long, radius this api will give the stations within that radius range.
        public List<StationModel> HomePage(string latitude, string longitude, string radius)
        {

            double latitudeD = Convert.ToDouble(latitude);
            double longitudeD = Convert.ToDouble(longitude);
            double radiusD = Convert.ToDouble(radius);

            List<StationModel> homePageStationList = new List<StationModel>();
            try
            {
                con = db.OpenConnection();
                cmd = new MySqlCommand("sp_homepage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("val_latitude", latitudeD);
                cmd.Parameters.AddWithValue("val_longitude", longitudeD);
                cmd.Parameters.AddWithValue("val_radius", radiusD);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    StationModel latlongstations = new StationModel();
                    latlongstations.stationId = sdr["id"].ToString();
                    latlongstations.stationName = sdr["station"].ToString();
                    latlongstations.aqi= sdr["aqi"].ToString();
                    latlongstations.latitude= sdr["latitude"].ToString();
                    latlongstations.longitude = sdr["longitude"].ToString();
                    homePageStationList.Add(latlongstations);

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }
            return homePageStationList;
        }

       

        //this api gets string in input and based on that returns the similar station names &  city,state,country names
        public SearchModel SearchList(string loc)
        {

            List<StationModel> stationNames = new List<StationModel>();
            
            List<LocationModel> cityNames = new List<LocationModel>();
            SearchModel objSearchList = new SearchModel();
            DataSet searchResult = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();

            try
            {
                con = db.OpenConnection();
                cmd = new MySqlCommand("sp_searchList", con);
                cmd.Parameters.AddWithValue("val_loc", loc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                da.SelectCommand = cmd;
                da.Fill(searchResult);




                for (int i = 0; i < searchResult.Tables[0].Rows.Count; i++)
                {

                    StationModel station = new StationModel();
                    station.stationId=searchResult.Tables[0].Rows[i].ItemArray[0].ToString();
                    station.stationName = searchResult.Tables[0].Rows[i].ItemArray[1].ToString();
                    station.aqi = searchResult.Tables[0].Rows[i].ItemArray[2].ToString();
                    station.latitude = searchResult.Tables[0].Rows[i].ItemArray[3].ToString();
                    station.longitude = searchResult.Tables[0].Rows[i].ItemArray[4].ToString();
                    station.lastUpdatedDate = searchResult.Tables[0].Rows[i].ItemArray[8].ToString();

                    stationNames.Add(station);


                }

                for (int i = 0; i < searchResult.Tables[1].Rows.Count; i++)
                {
                    LocationModel city = new LocationModel();
                    city.cityId = searchResult.Tables[1].Rows[i].ItemArray[0].ToString();
                    city.city = searchResult.Tables[1].Rows[i].ItemArray[1].ToString();
                    city.state = searchResult.Tables[1].Rows[i].ItemArray[2].ToString();
                    city.country = searchResult.Tables[1].Rows[i].ItemArray[3].ToString();
                    cityNames.Add(city);

                }
                
                objSearchList.stationList = stationNames;
                objSearchList.cityList = cityNames;
               
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }
            return objSearchList;
        }

        //if user selects city then this id will give list of all stations, aqi value, lat,long to plot in graph
        public List<StationModel> StationInCity(string id)
        {
            int cityId = Int16.Parse(id);
            

            List<StationModel> StationInCityList = new List<StationModel>();
            try
            {
                
                con = db.OpenConnection();
                cmd = new MySqlCommand("sp_stationInCity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("val_cityId", cityId);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    StationModel latlongstations = new StationModel();
                    latlongstations.stationId = sdr["id"].ToString();
                    latlongstations.stationName = sdr["station"].ToString();
                    latlongstations.aqi = sdr["aqi"].ToString();
                    latlongstations.latitude = sdr["latitude"].ToString();
                    latlongstations.longitude = sdr["longitude"].ToString();
                    StationInCityList.Add(latlongstations);

                }

                sdr.Close();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }
            return StationInCityList;
        }


        //we get the station id and give back the 
        //aqi value of that station, station name
        //pollutant names ,pollutant values for current time
        //date, aqi max value for last 7 days
        public StationHomePageModel stationHomePage(string Id)
        {
            int StationId = Int16.Parse(Id);
            DataSet stationResult = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();

            StationModel sm = new StationModel();


            StationHomePageModel StationHomePage = new StationHomePageModel();
            List<PollutantModel> pmList = new List<PollutantModel>();
            List<AqiHistoryModel> amList = new List<AqiHistoryModel>();
            try
            {

                con = db.OpenConnection();
                cmd = new MySqlCommand("sp_stationData", con);
                cmd.Parameters.AddWithValue("val_stationId", StationId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();               
                da.SelectCommand = cmd;
                da.Fill(stationResult);


         

                for (int i = 0; i < stationResult.Tables[0].Rows.Count; i++)
                {
                    sm.stationId= stationResult.Tables[0].Rows[i].ItemArray[0].ToString();
                    sm.stationName = stationResult.Tables[0].Rows[i].ItemArray[1].ToString();
                    sm.aqi = stationResult.Tables[0].Rows[i].ItemArray[2].ToString();
                    sm.latitude = stationResult.Tables[0].Rows[i].ItemArray[3].ToString();
                    sm.longitude = stationResult.Tables[0].Rows[i].ItemArray[4].ToString();
                    sm.lastUpdatedDate = stationResult.Tables[0].Rows[i].ItemArray[5].ToString();
                    sm.city = stationResult.Tables[0].Rows[i].ItemArray[6].ToString();
                    sm.state = stationResult.Tables[0].Rows[i].ItemArray[7].ToString();
                    sm.country = stationResult.Tables[0].Rows[i].ItemArray[8].ToString();

                }

                for (int i = 0; i < stationResult.Tables[1].Rows.Count; i++)
                {
                    PollutantModel pm = new PollutantModel();
                    pm.pollutantName = stationResult.Tables[1].Rows[i].ItemArray[0].ToString();
                    pm.pollutantValue= stationResult.Tables[1].Rows[i].ItemArray[1].ToString();
                    pm.lastUpdatedDate = stationResult.Tables[1].Rows[i].ItemArray[2].ToString();
                    pmList.Add(pm);

                }

                for (int i = 0; i < stationResult.Tables[2].Rows.Count; i++)
                {
                    AqiHistoryModel am = new AqiHistoryModel();
                    am.aqiMaxValue= stationResult.Tables[2].Rows[i].ItemArray[0].ToString();
                    am.Date = stationResult.Tables[2].Rows[i].ItemArray[1].ToString();
                    amList.Add(am);

                }

               
                StationHomePage.AqiMaxValueList = amList;
                StationHomePage.PollutantValueList = pmList;

                StationHomePage.StationData = sm;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }
            return StationHomePage;
        }

        //we get the station id and pollutant name  then give back the 
        //max polllutant value upto 7 days for that station id each day
        public List<PollutantModel> PollutantHistory(string stationId, string pollutantName)
        {



            List<PollutantModel> pollutantHistoryList = new List<PollutantModel>();
            try
            {
                con = db.OpenConnection();
                cmd = new MySqlCommand("sp_pollutantHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("val_stationId", stationId);
                cmd.Parameters.AddWithValue("val_pollutantName", pollutantName);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    PollutantModel pm = new PollutantModel();
                    pm.pollutantValue = sdr["mvalue"].ToString();
                    pm.lastUpdatedDate = sdr["created_date"].ToString();
                    pollutantHistoryList.Add(pm);

                }
            }
                       
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }
            return pollutantHistoryList;
        }



        //this api returns the two most polluted station with their city name in particular country and 
        //two most healthy station with their city name  of that country.
        public HigestLowestAqi HighestLowestStationAqi(string countryName)
        {

            

            DataSet stationResult = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();

           
          


            HigestLowestAqi HighLowAqi = new HigestLowestAqi();
            List<StationModel> HighStationList = new List<StationModel>();
            List<StationModel> LowStationList = new List<StationModel>();
            try
            {

                con = db.OpenConnection();
                cmd = new MySqlCommand("sp_HighestLowestStationAqi", con);
                cmd.Parameters.AddWithValue("val_countryName", countryName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                da.SelectCommand = cmd;
                da.Fill(stationResult);



                if (stationResult.Tables[0].Rows.Count > 0)
                {
                    HighLowAqi.MessageHigh = "Most polluted cities found";

                    for (int i = 0; i < stationResult.Tables[0].Rows.Count; i++)
                    {

                        StationModel smHigh = new StationModel();
                      
                        smHigh.stationId = stationResult.Tables[0].Rows[i].ItemArray[0].ToString();
                        smHigh.stationName = stationResult.Tables[0].Rows[i].ItemArray[1].ToString();
                        smHigh.aqi = stationResult.Tables[0].Rows[i].ItemArray[2].ToString();
                        smHigh.latitude = stationResult.Tables[0].Rows[i].ItemArray[3].ToString();
                        smHigh.longitude = stationResult.Tables[0].Rows[i].ItemArray[4].ToString();
                        smHigh.lastUpdatedDate = stationResult.Tables[0].Rows[i].ItemArray[5].ToString();
                        smHigh.city = stationResult.Tables[0].Rows[i].ItemArray[6].ToString();
                        smHigh.state = stationResult.Tables[0].Rows[i].ItemArray[7].ToString();
                        smHigh.country = stationResult.Tables[0].Rows[i].ItemArray[8].ToString();

                        HighStationList.Add(smHigh);
                      

                    }
                }
                else
                {
                    HighLowAqi.MessageHigh = "Most polluted cities Not found";
                }

                if (stationResult.Tables[1].Rows.Count > 0)
                {
                    HighLowAqi.MessageLow = "Most Healthy city found";

                    for (int i = 0; i < stationResult.Tables[1].Rows.Count; i++)
                    {
                        StationModel smLow = new StationModel();
                        smLow.stationId = stationResult.Tables[1].Rows[i].ItemArray[0].ToString();
                        smLow.stationName = stationResult.Tables[1].Rows[i].ItemArray[1].ToString();
                        smLow.aqi = stationResult.Tables[1].Rows[i].ItemArray[2].ToString();
                        smLow.latitude = stationResult.Tables[1].Rows[i].ItemArray[3].ToString();
                        smLow.longitude = stationResult.Tables[1].Rows[i].ItemArray[4].ToString();
                        smLow.lastUpdatedDate = stationResult.Tables[1].Rows[i].ItemArray[5].ToString();
                        smLow.city = stationResult.Tables[1].Rows[i].ItemArray[6].ToString();
                        smLow.state = stationResult.Tables[1].Rows[i].ItemArray[7].ToString();
                        smLow.country = stationResult.Tables[1].Rows[i].ItemArray[8].ToString();
                        LowStationList.Add(smLow);
                      

                    }

                }
                else
                {
                    HighLowAqi.MessageLow = "Most Healthy city not found";
                }

                HighLowAqi.TwoMostHealthyCities = LowStationList;
                HighLowAqi.TwoMostPollutedCities = HighStationList;
               
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }
            return HighLowAqi;
        }

    }
}