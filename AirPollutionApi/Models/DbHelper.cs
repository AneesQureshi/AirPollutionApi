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

        //from the users location lat, long, radius this api will give the stations within that radius range.
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
            List<StationModel> stationNamesInCity = new List<StationModel>();
            List<LocationModel> cityNames = new List<LocationModel>();
            SearchModel objSearchList = new SearchModel();
           

            try
            {


                con = db.OpenConnection();
                cmd = new MySqlCommand("sp_locationStation", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("val_loc", loc);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    StationModel station = new StationModel();
                    station.stationId = sdr["id"].ToString();
                    station.stationName = sdr["station"].ToString();
                    station.aqi = sdr["aqi"].ToString();
                    station.latitude = sdr["latitude"].ToString();
                    station.longitude = sdr["longitude"].ToString();
                    station.lastUpdatedDate = sdr["updated_date"].ToString();
                   
                    stationNames.Add(station);

                }

                sdr.Close();

             
                cmd = new MySqlCommand("sp_locationCity", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("val_loc", loc);
                MySqlDataReader sdr1 = cmd.ExecuteReader();

                while (sdr1.Read())
                {
                    LocationModel city = new LocationModel();
                    city.cityId = sdr1["id"].ToString();
                    city.city = sdr1["city"].ToString();
                    city.state = sdr1["state"].ToString();
                    city.country = sdr1["country"].ToString();

                    stationNamesInCity= StationInCity(city.cityId);
                    cityNames.Add(city);
                 
                }

                sdr1.Close();


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
                    string aqi = stationResult.Tables[0].Rows[i].ItemArray[0].ToString();
                    string station = stationResult.Tables[0].Rows[i].ItemArray[1].ToString();
                    string created_date = stationResult.Tables[0].Rows[i].ItemArray[2].ToString();
                    sm.aqi = aqi;
                    sm.stationName = station;
                    sm.lastUpdatedDate = created_date;
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


    }
}