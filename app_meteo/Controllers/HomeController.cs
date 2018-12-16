using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Tempex.Models;
using GeoCoordinatePortable;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Tempex.Controllers;
using System.Web.Helpers;

namespace app_meteo.Controllers
{
    public class HomeController : Controller
    {
        public static string str = ConfigurationManager.ConnectionStrings["TempexDB"].ConnectionString;

        public ActionResult Index()
        {
            List<string> cities = WebCache.Get("CachedCities");

            if (cities == null)
            {
                cities = new List<string>();
                string homeCity = string.Empty;
                List<Coords> coords = new List<Coords>();
                string ipAddress = ControllerContext.HttpContext.Request.UserHostAddress;
                //string ipAddress = "85.245.44.51";
                string json = "";
                using (var httpClient = new HttpClient())
                {
                    json = httpClient.GetStringAsync("http://api.ipstack.com/" + ipAddress + "?access_key=e4d00fc4a247fb9307e3b29da42d11df").Result;
                }
                JObject jObject = JObject.Parse(json);
                Coords hostCoords = new Coords("", (double)jObject["latitude"], (double)jObject["longitude"]);

                using (SqlConnection conn = new SqlConnection(str))
                {
                    try
                    {
                        conn.Open();
                        string sql = "SELECT * FROM Location";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    coords.Add(new Coords((string)reader["Name"], Convert.ToDouble(reader["Lat"]), Convert.ToDouble(reader["Lon"])));
                                }
                            }
                        }
                        double initialDistance = 100000000;
                        var sCoord = new GeoCoordinate(hostCoords.lat, hostCoords.lon);
                        foreach (var item in coords)
                        {
                            var eCoord = new GeoCoordinate(item.lat, item.lon);
                            double distance = sCoord.GetDistanceTo(eCoord);
                            if (distance < initialDistance)
                            {
                                initialDistance = distance;
                                homeCity = item.city;
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        conn.Close();
                        homeCity = "Lisbon";
                    }
                    conn.Close();
                }
                cities.Add(homeCity);
            }
            ViewBag.CitiesList = cities;
            WebCache.Set("CachedCities", cities, 7200, true);
            return View();
        }

        public ActionResult Card(string id, int? add)
        {
            List<Day> forecast = ClassAuxiliar.GetForecast(id);

            char first = char.ToUpper(id[0]);
            string newString = id.Remove(0, 1).Insert(0, first.ToString());

            ViewBag.City = newString;
            ViewBag.Image = forecast[0].icon;
            ViewBag.Condition = forecast[0].text;
            ViewBag.Max = forecast[0].maxtemp_c;
            ViewBag.Min = forecast[0].mintemp_c;
            ViewBag.Hum = forecast[0].avghumidity;

            if(add != null)
            {
                List<string> cities = WebCache.Get("CachedCities");
                cities.Add(id);
                WebCache.Set("CachedCities", cities, 4320, true);
            }

            return PartialView("_Card");
        }

        public EmptyResult RemoveCity(string id)
        {
            List<string> cities = WebCache.Get("CachedCities");
            if(cities.Count > 1)
            {
                cities.Remove(id);
                WebCache.Set("CachedCities", cities, 7200, true);
            }
            
            return null;
        }

    }
}