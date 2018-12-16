using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tempex.Models;

namespace Tempex.Controllers
{
    public class ForecastController : Controller
    {
        // GET: CincoDias
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult cincoDiasCard(string id)
        {
            List<Day> forecast = ClassAuxiliar.GetForecast(id);

            ViewBag.Forecast = forecast;

            return PartialView("_CardCincoDias");
        }
    }
}