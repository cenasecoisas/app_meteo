using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TempexAPI.Models;

namespace TempexAPI.Controllers
{
    public class CityController : ApiController
    {
        public NewRootObject Get(String id)
        {
            String json = null;
            RootObject oldObject = new RootObject();
            using (TempexDBEntities context = new TempexDBEntities())
            {
                var query = from c in context.jsonAPI
                            where c.CityID.Contains(id)
                            select c;
                foreach (var c in query) 
                {
                    json = c.jsonString;
                }

                oldObject = JsonConvert.DeserializeObject<RootObject>(json);
            }

            NewRootObject newObject = new NewRootObject();

            NewLocationObject newLocation = new NewLocationObject
            {
                country = oldObject.location.country,
                lat = oldObject.location.lat,
                lon = oldObject.location.lon,
                localtime = oldObject.location.localtime,
                name = oldObject.location.name
            };

            newObject.location = newLocation;

            NewForecastObject newForecast = new NewForecastObject();
            List<NewForecastdayObject> newList = new List<NewForecastdayObject>();

            foreach (var item in oldObject.forecast.forecastday)
            {
                NewForecastdayObject newForecastday = new NewForecastdayObject();

                NewDayObject newDay = new NewDayObject
                {
                    maxtemp_c = item.day.maxtemp_c,
                    mintemp_c = item.day.mintemp_c,
                    avghumidity = item.day.avghumidity,
                    condition = item.day.condition
                };

                newForecastday.day = newDay;
                newForecastday.date = item.date;
                
                newList.Add(newForecastday);
            }
            newForecast.forecastday = newList;

            newObject.forecast = newForecast;
            return newObject;
        }

        public List<Coords> GetCoords()
        {
            String json = null;
            RootObject rootObject = new RootObject();
            List<Coords> coords = new List<Coords>();
            using (TempexDBEntities context = new TempexDBEntities())
            {
                var query = from c in context.jsonAPI
                            select c;
                foreach (var c in query)
                {
                    json = c.jsonString;
                    rootObject = JsonConvert.DeserializeObject<RootObject>(json);
                    coords.Add(new Coords
                    {
                        city = rootObject.location.name,
                        lat = rootObject.location.lat,
                        lon = rootObject.location.lon
                    });
                }
            }
            return coords;
        }
    }
}
