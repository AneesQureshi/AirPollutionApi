using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class DbConnect
    {

        MySqlConnection con;
        public DbConnect()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new MySqlConnection(connectionString);
        }

        public MySqlConnection OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        public MySqlConnection CloseConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return con;
        }

    }
}