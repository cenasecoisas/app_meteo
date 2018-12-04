using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApixuFunctionApp.Models
{
    public class Location
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string tz_id { get; set; }
        public int localtime_epoch { get; set; }
        public string localtime { get; set; }
    }

    public class Condition
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }

    public class Current
    {
        public Condition condition { get; set; }
        public double uv { get; set; }
    }

    public class Day
    {
        public double maxtemp_c { get; set; }
        public double mintemp_c { get; set; }
        public double avghumidity { get; set; }
        public Condition condition { get; set; }
        public double uv { get; set; }
    }

    public class Astro
    {
    }

    public class Forecastday
    {
        public string date { get; set; }
        public Day day { get; set; }
        public Astro astro { get; set; }
    }

    public class Forecast
    {
        public List<Forecastday> forecastday { get; set; }
    }

    public class RootObject
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }
}