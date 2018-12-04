using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempexAPI.Models
{
    public class NewRootObject
    {
        public NewLocationObject location { get; set; }
        public NewForecastObject forecast { get; set; }
    }
}