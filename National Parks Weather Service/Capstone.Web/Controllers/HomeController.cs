using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.DAL;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
        private IParksSqlDAL _parksDal;

        public HomeController(IParksSqlDAL parksDal)
        {
            this._parksDal = parksDal;
        }

        // GET: Home
        public ActionResult Index()
        {
            var parks = _parksDal.RetrieveParks();
            return View("Index", parks);
        }

        public ActionResult Detail(string parkCode)
        {
            Park park = _parksDal.GetOnePark(parkCode);
            List<Weather> weather = new List<Weather>();
            if (Session["WeatherData"] == null)
            {
                 weather = _parksDal.RetrieveOneParkWeather(parkCode);
            }
            else
            {
                if ((string)Session["ParkCode"] == parkCode)
                {
                    weather = (List<Weather>)Session["WeatherData"];
                }
                else
                {
                    int scale = 0;
                    if ((string)Session["TempScale"] == "Celsius")
                    {
                        scale = 1;
                    }

                    return RedirectToAction("TempScaleChoice", new {scale = scale, parkCode = parkCode });
                }
            }
            ParkDetail parkDetail = new ParkDetail();
            parkDetail.PopulateParkProperties(park);
            parkDetail.PopulateForecast(weather);
            return View("Detail", parkDetail);
        }

        public ActionResult WeatherDetail()
        {
            List<Weather> weather = _parksDal.PopulateWeather();

            if (Session["TempScale"] != null && (string)Session["TempScale"] != "Fahrenheit")
            {
                //Displays all temps as Celsius
                foreach (var item in weather)
                {
                    item.HighTemp = (item.HighTemp - 32) * (5 / 9);
                    item.LowTemp = (item.LowTemp - 32) * (5 / 9);
                }
            }

            return View("WeatherDetail", weather);
        }

        public ActionResult FavoriteParks()
        {
            var surveys = _parksDal.GetSurveysPerPark();
            return View("FavoriteParks", surveys);
        }

        public ActionResult Survey()
        {
            SurveyViewModel survey = new SurveyViewModel();
            survey.Parks = _parksDal.RetrieveParks();

            return View("Survey", survey);
        }
        
        public ActionResult TempScaleChoice(int scale, string parkCode)
        {

            List<Weather> weather = _parksDal.RetrieveOneParkWeather(parkCode);

            if (scale == 1)
            {
                Session["TempScale"] = "Celsius";
                foreach (var item in weather)
                {
                    item.HighTemp = Convert.ToInt32((item.HighTemp - 32) * (5 / 9.0));
                    item.LowTemp = Convert.ToInt32((item.LowTemp - 32) * (5 / 9.0));
                    item.TempScale = "°C";
                }
            }
            else
            {
                Session["TempScale"] = "Fahrenheit";
                foreach(var item in weather)
                {
                    item.TempScale = "°F";
                }
            }

            Session["WeatherData"] = weather;
            Session["ParkCode"] = parkCode;

            return Detail(parkCode);
        }

        [HttpPost]
        public ActionResult NewSurvey(string Park, string EmailAddress, string state, string activityLevel)
        {
            Survey newSurvey = new Survey();
            newSurvey.ParkCode = Park;
            newSurvey.Email = EmailAddress;
            newSurvey.State = state;
            newSurvey.ActivityLevel = activityLevel;
            _parksDal.SaveSurvey(newSurvey);

            var allSurveys = _parksDal.GetAllSurveys();
            return RedirectToAction("FavoriteParks", allSurveys);
        }
    }
}