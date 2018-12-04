using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tempex.Models
{
    public class Coords
    {
        public string city { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }

        public Coords(string city, double lat, double lon)
        {
            this.city = city;
            this.lat = lat;
            this.lon = lon;
        }
    }
}