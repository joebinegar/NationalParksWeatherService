using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Controllers;
using Capstone.Web.DAL;

namespace Capstone.Web.Models
{
    public class Weather
    {
        public string ParkCode { get; set; }
        public int ForecastValue { get; set; }
        public int LowTemp { get; set; }
        public int HighTemp { get; set; }
        public string Forecast { get; set; }
        public string TempRecommendation { get
            {
                return TempRec(HighTemp, LowTemp);
            } }
        public string WeatherRecommendation { get
            {
                return WeatherRec(Forecast);
            } }
        public string TempScale { get; set; } = "°F";

        public int LowTempCelcius
        {
            get
            {
                LowTemp = Convert.ToInt32((LowTemp - 32) * (5 / 9.0));
                return LowTemp;
            }
        }

        public int HighTempCelcius
        {
            get
            {
                HighTemp = Convert.ToInt32((HighTemp - 32) * (5 / 9.0));
                return HighTemp;
            }
        }
        /// <summary>
        /// Returns list of appropriate strings to call to display daily recommendations for user
        /// </summary>
        /// <returns></returns>
        /// 
        public string TempRec(int high, int low)
        {
            string result = "";
            if (TempScale == "°C")
            {
                if (high > 23)
                {
                    result = "Bring an extra gallon of water!";
                }
                else if (low + 36 < high)
                {
                    result = "Wear breathable layers!";
                }
                else if (low < -6)
                {
                    result = "Remember it's dangerous for extended exposure to frigid temperatures";
                }
            }
            else
            {
                if (high > 75)
                {
                    result = "Bring an extra gallon of water!";
                }
                else if (low + 20 < high)
                {
                    result = "Wear breathable layers!";
                }
                else if (low < 20)
                {
                    result = "Remember it's dangerous for extended exposure to frigid temperatures";
                }
            }

            return result;
        }

        public string WeatherRec(string forecast)
        {
            string result = "";
            if (Forecast == "snow")
            {
                result = "Pack snowshoes!";
            }
            else if (Forecast == "rain")
            {
                result = "Pack rain gear with waterproof shoes";
            }
            else if (Forecast == "thunderstorms")
            {
                result = "Seek shelter! Avoid hiking on exposed ridges";
            }
            else if (Forecast == "sunny")
            {
                result = "Pack sunblock";
            }
                return result;
        }

        public List<string> DisplayRecommendations()
        {
            List<string> recommendations = new List<string>();
            if (IsSnowing())
            {
                recommendations.Add("Pack snowshoes!");
            }
            if (IsRaining())
            {
                recommendations.Add("Pack rain gear with waterproof shoes.");
            }
            if (IsThunderStorming())
            {
                recommendations.Add("Seek shelter! Avoid hiking on exposed ridges.");
            }
            if (IsSunny())
            {
                recommendations.Add("Pack sunblock.");
            }
            if (IsWarm())
            {
                recommendations.Add("Bring an extra gallon of water!");
            }
            if (IsVariableTemp())
            {
                recommendations.Add("Wear breathable layers!");
            }
            if (IsCold())
            {
                recommendations.Add("Remember it's dangerous for extended exposure to frigid temperatures.");
            }

            return recommendations;
        }

        private bool IsSnowing()
        {
            bool isTrue = false;

            if(Forecast == "snow")
            {
                isTrue = true;
            }
            return isTrue;
        }

        private bool IsRaining()
        {
            bool isTrue = false;

            if (Forecast == "rain")
            {
                isTrue = true;
            }
            return isTrue;
        }

        private bool IsThunderStorming()
        {
            bool isTrue = false;

            if (Forecast == "thunderstorms")
            {
                isTrue = true;
            }
            return isTrue;
        }

        private bool IsSunny()
        {
            bool isTrue = false;

            if (Forecast == "sunny")
            {
                isTrue = true;
            }
            return isTrue;
        }

        private bool IsWarm()
        {
            bool isTrue = false;

            if (HighTemp > 75)
            {
                isTrue = true;
            }
            return isTrue;
        }

        private bool IsVariableTemp()
        {
            bool isTrue = false;

            if (LowTemp + 20 < HighTemp)
            {
                isTrue = true;
            }
            return isTrue;
        }

        private bool IsCold()
        {
            bool isTrue = false;

            if (LowTemp < 20)
            {
                isTrue = true;
            }
            return isTrue;
        }


    }
}