using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempexAPI.Models
{
    public class Forecastday
    {
        public string date { get; set; }
        public int date_epoch { get; set; }
        public Day day { get; set; }
        public Astro astro { get; set; }
    }
}