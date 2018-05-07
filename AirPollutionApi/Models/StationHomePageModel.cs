using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPollutionApi.Models
{
    public class StationHomePageModel
    {
        public StationModel StationData { get; set; }
        public List<PollutantModel>  PollutantValueList { get; set; }
        public List<AqiHistoryModel> AqiMaxValueList { get; set; }

    }
}