using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Tempex.Models;
using Newtonsoft.Json;
using GeoCoordinatePortable;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace app_meteo.Controllers
{
    public class HomeController : Controller
    {
        public static string str = ConfigurationManager.ConnectionStrings["TempexDB"].ConnectionString;

        public ActionResult Index()
        {
            string city = "";
            List<Coords> coords = new List<Coords>();
            //string ipAddress = ControllerContext.HttpContext.Request.UserHostAddress;
            string ipAddress = "85.245.44.51";
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    conn.Close();
                }
                conn.Close();

                double initialDistance = 100000000;
                var sCoord = new GeoCoordinate(hostCoords.lat, hostCoords.lon);
                foreach (var item in coords)
                {
                    var eCoord = new GeoCoordinate(item.lat, item.lon);
                    double distance = sCoord.GetDistanceTo(eCoord);
                    if (distance < initialDistance)
                    {
                        initialDistance = distance;
                        city = item.city;
                    }
                };
            }

            List<Day> forecast = GetForecast(city);

            ViewBag.City = city;
            ViewBag.Image = forecast[0].icon;
            ViewBag.Condition = forecast[0].text;
            ViewBag.Max = forecast[0].maxtemp_c;
            ViewBag.Min = forecast[0].mintemp_c;
            ViewBag.Hum = forecast[0].avghumidity;

            return View();
        }

        public List<Day> GetForecast(string city)
        {
            List<Day> forecast = new List<Day>();
            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM Day where ID Like ('" + city + "%')";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Day day = new Day();
                                day.maxtemp_c = Math.Round(Convert.ToDouble(reader["Maxtemp_c"]),1);
                                day.mintemp_c = Math.Round(Convert.ToDouble(reader["Mintemp_c"]),1);
                                day.avghumidity = Math.Round(Convert.ToDouble(reader["Avghumidity"]),1);
                                day.text = (string)reader["Text"];
                                day.icon = (string)reader["Icon"];
                                day.code = Convert.ToInt32(reader["Code"]);
                                forecast.Add(day);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    conn.Close();
                }
                conn.Close();
            }
            return forecast;
        }

        public ActionResult Card(string id)
        {

            ViewBag.City = "city";
            ViewBag.Image = "";
            ViewBag.Condition = "condition";
            ViewBag.Max = "max temp";
            ViewBag.Min = "min temp";
            ViewBag.Hum = "humidity";

            return PartialView("_Card");
        }
    }
}