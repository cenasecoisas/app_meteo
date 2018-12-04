using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempexAPI.Models
{
    public class NewLocationObject
    {
        public string name { get; set; }
        public string country { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string localtime { get; set; }
    }
}