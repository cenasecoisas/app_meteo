using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempexAPI.Models
{
    public class Forecast
    {
        public List<Forecastday> forecastday { get; set; }
    }
}