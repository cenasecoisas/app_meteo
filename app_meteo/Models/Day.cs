using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tempex.Models
{
    public class Day
    {
        public double maxtemp_c { get; set; }
        public double mintemp_c { get; set; }
        public double avghumidity { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }
}