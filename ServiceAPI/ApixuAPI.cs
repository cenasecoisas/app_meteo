using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAPI
{
    public partial class ApixuAPI : ServiceBase
    {
        public ApixuAPI()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("http://api.apixu.com/v1/forecast.json?key=bd729b976092477fbdf81833180705&q=lisboa&days=5").Result;

            if (response.IsSuccessStatusCode)
            {
                string JSON = response.Content.ReadAsStringAsync().Result;
                SqlDataAdapter adapter = new SqlDataAdapter();
                string connetionString = "Data Source=DESKTOP-2HBATFA/SQLEXPRESS;Initial Catalog=TempexDB;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connetionString);
                string sql = string.Format("insert into TempexTable (City,JSON,lastUpdate) values('Lisboa',{0},600)", JSON);
                try
                {
                    connection.Open();
                    adapter.InsertCommand = new SqlCommand(sql, connection);
                    adapter.InsertCommand.ExecuteNonQuery();
                } catch (Exception ex)
                {
                    
                } finally {
                    connection.Close();
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
