using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempexAPI.Models
{
    public class NewForecastdayObject
    {
        public string date { get; set; }
        public NewDayObject day { get; set; }
    }
}