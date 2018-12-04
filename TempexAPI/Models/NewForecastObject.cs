using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempexAPI.Models
{
    public class NewForecastObject
    {
        public List<NewForecastdayObject> forecastday { get; set; }
    }
}