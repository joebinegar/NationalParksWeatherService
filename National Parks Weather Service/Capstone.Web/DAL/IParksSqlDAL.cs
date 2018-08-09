using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
    public interface IParksSqlDAL
    {
        List<Park> RetrieveParks();
        List<Weather> PopulateWeather();
        List<Survey> GetAllSurveys();
        bool SaveSurvey(Survey survey);
        List<Weather> RetrieveOneParkWeather(string parkCode);
        Park GetOnePark(string parkCode);
        List<Survey> GetSurveysPerPark();
    }
}
