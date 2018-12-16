using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Tempex.Models;

namespace Tempex.Controllers
{
    public class ClassAuxiliar
    {
        public static string str = ConfigurationManager.ConnectionStrings["TempexDB"].ConnectionString;

        public static List<Day> GetForecast(string city)
        {
            List<Day> forecast = new List<Day>();
            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM Day where DayCode Like ('" + city + "%')";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Day day = new Day();
                                day.maxtemp_c = Math.Round(Convert.ToDouble(reader["Maxtemp_c"]), 1);
                                day.mintemp_c = Math.Round(Convert.ToDouble(reader["Mintemp_c"]), 1);
                                day.avghumidity = Math.Round(Convert.ToDouble(reader["Avghumidity"]), 1);
                                day.text = (string)reader["Text"];
                                day.icon = (string)reader["Icon"];
                                day.code = Convert.ToInt32(reader["Code"]);
                                day.date = (DateTime)reader["Date"];
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

    }
}