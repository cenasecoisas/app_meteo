using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using ApixuFunctionApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ApixuFunctionApp
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static void Run([TimerTrigger("0 06 00 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            string[] cities = File.ReadAllLines(@"D:\home\site\wwwroot\Function2\cities.txt");

            foreach (var city in cities)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("http://api.apixu.com/v1/forecast.json?key=bd729b976092477fbdf81833180705&q=" + city + "&days=5").Result;

                if (response.IsSuccessStatusCode)
                {
                    log.Info($"----- {city} -----");
                    string JSON = response.Content.ReadAsStringAsync().Result;
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(JSON);
                    var str = Environment.GetEnvironmentVariable("sqldb_connection");

                    FunctionMethod.Do(str, city, rootObject, log);
                }
                else
                {
                    log.Info($"{city} non exist");
                }
            }

        }
    }
}
