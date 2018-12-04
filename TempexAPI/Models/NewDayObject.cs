using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempexAPI.Models
{
    public class NewDayObject
    {
        public double maxtemp_c { get; set; }
        public double mintemp_c { get; set; }
        public double avghumidity { get; set; }
        public Condition condition { get; set; }
    }
}