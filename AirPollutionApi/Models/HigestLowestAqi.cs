using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class HigestLowestAqi
    {
        public string MessageLow { get; set; }
        public List<StationModel> TwoMostHealthyCities { get; set; }
        public string MessageHigh { get; set; }
        public List<StationModel> TwoMostPollutedCities { get; set; }
    }
}