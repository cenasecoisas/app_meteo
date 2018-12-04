using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApixuFunctionApp.Models
{
    class FunctionMethod
    {
        public static void Do(string str, string city, RootObject rootObject, TraceWriter log)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE Location SET Country = @country, Lat = @lat, Lon = @lon " +
                        "WHERE Name LIKE @name IF @@ROWCOUNT=0 " +
                        "INSERT INTO Location (Name,Country,Lat,Lon) values (@name,@country,@lat,@lon)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", city);
                        cmd.Parameters.AddWithValue("@country", rootObject.location.country);
                        cmd.Parameters.AddWithValue("@lat", rootObject.location.lat);
                        cmd.Parameters.AddWithValue("@lon", rootObject.location.lon);
                        cmd.ExecuteNonQuery();
                    }

                    int count = 1;
                    foreach (var obj in rootObject.forecast.forecastday)
                    {
                        sql = "UPDATE Day SET Maxtemp_c = @maxtemp_c, Mintemp_c = @mintemp_c, Avghumidity = @avghumidity, Text = @text, Icon = @icon, Code = @code, Date = @date " +
                            "WHERE ID LIKE @id IF @@ROWCOUNT=0 " +
                            "INSERT INTO Day (ID,Maxtemp_c,Mintemp_c,Avghumidity,Text,Icon,Code,Date) values(@id,@maxtemp_c,@mintemp_c,@avghumidity,@text,@icon,@code,@date)";
                        string dayID = city + count;
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", dayID);
                            cmd.Parameters.AddWithValue("@maxtemp_c", obj.day.maxtemp_c);
                            cmd.Parameters.AddWithValue("@mintemp_c", obj.day.mintemp_c);
                            cmd.Parameters.AddWithValue("@avghumidity", obj.day.avghumidity);
                            cmd.Parameters.AddWithValue("@text", obj.day.condition.text);
                            cmd.Parameters.AddWithValue("@icon", obj.day.condition.icon);
                            cmd.Parameters.AddWithValue("@code", obj.day.condition.code);
                            cmd.Parameters.AddWithValue("@date", obj.date);
                            cmd.ExecuteNonQuery();
                        }
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    log.Info($"Exception: {ex.Message}");
                    conn.Close();
                }
                conn.Close();
            }
        }
    }
}
