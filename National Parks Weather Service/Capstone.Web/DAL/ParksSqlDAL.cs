using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
    public class ParksSqlDAL : IParksSqlDAL
    {
        private string _connectionString;

        public ParksSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves all the park information from the database
        /// </summary>
        /// <returns></returns>
        public List<Park> RetrieveParks()
        {
            List<Park> parks = new List<Park>();

            string parksSearchSql = @"SELECT * FROM park";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(parksSearchSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        parks.Add(MapRowToParks(reader));
                    }
                }

                return parks;
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves ALL weather data for all parks from the database
        /// </summary>
        /// <returns>Weather for all parks</returns>
        public List<Weather> PopulateWeather()
        {
            List<Weather> weather = new List<Weather>();

            string parksSearchSql = @"SELECT * FROM weather";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(parksSearchSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        weather.Add(MapRowToWeather(reader));
                    }
                }

                return weather;
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets weather for selected park
        /// </summary>
        /// <returns>Selected park weather</returns>
        public Weather FiveDayForecast(string parkCode)
        {
            Weather output = new Weather();

            string parkWeather = @"select * from weather WHERE parkCode = @parkCode;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = parkWeather;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    output = MapRowToWeather(reader);
                }
            }

            return output;
        }

        /// <summary>
        /// Retrieves a single selected park information
        /// </summary>
        /// <returns></returns>
        public Park GetOnePark(string parkCode)
        {
            Park park = new Park();

            string getPark = @"select * from park WHERE parkCode = @parkCode;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = getPark;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    park = MapRowToParks(reader);
                }
            }

            return park;
        }


        /// <summary>
        /// Retrieves the weather for the selected park
        /// </summary>
        /// <param name="park"></param>
        /// <returns></returns>
        public List<Weather> RetrieveOneParkWeather(string parkCode)
        {
            List<Weather> weather = new List<Weather>();

            string parksSearchSql = @"SELECT * FROM weather WHERE parkCode = @parkCode;";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(parksSearchSql, conn);
                    cmd.Parameters.AddWithValue("@parkCode", parkCode);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        weather.Add(MapRowToWeather(reader));
                    }
                }

                return weather;
            }
            catch (SqlException ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Retrieves all the surveys from the database
        /// </summary>
        /// <returns></returns>
        public List<Survey> GetAllSurveys()
        {

            List<Survey> surveys = new List<Survey>();

            string postSearchSql = @"SELECT * FROM survey_result;"; 
            //need to exclude parks where number of surveys are <= 1;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = postSearchSql;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Survey survey = MapRowToSurvey(reader);
                    surveys.Add(survey);
                }
            }

            return surveys;
        }
        /// <summary>
        /// Retrieves the number of surveys per each park in database
        /// </summary>
        /// <returns></returns>
        public List<Survey> GetSurveysPerPark()
        {

            List<Survey> surveys = new List<Survey>();

            string postSearchSql = @"SELECT COUNT(surveyId) AS numSurveys, survey_result.parkCode, park.parkName AS parkName 
                                     FROM survey_result JOIN park
                                     ON survey_result.parkCode = park.parkCode 
                                     GROUP BY survey_result.parkCode, parkName 
                                     ORDER BY numSurveys DESC;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = postSearchSql;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Survey survey = MapRowToSurveyCount(reader);
                    surveys.Add(survey);
                }
            }

            return surveys;
        }
        /// <summary>
        /// Saves new park survey to database
        /// </summary>
        /// <param name="newSurvey"></param>
        /// <returns></returns>
        public bool SaveSurvey(Survey newSurvey)
        {
            bool isTrue = false;
            string SQL_InsertSurvey = "INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) " +
                                                "VALUES (@parkCode, @email, @state, " +
                                                "@activityLevel);";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertSurvey, conn);
                    //cmd.Parameters.AddWithValue("@parkID", )
                    cmd.Parameters.AddWithValue("@parkCode", newSurvey.ParkCode);
                    cmd.Parameters.AddWithValue("@email", newSurvey.Email);
                    cmd.Parameters.AddWithValue("@state", newSurvey.State);
                    cmd.Parameters.AddWithValue("@activityLevel", newSurvey.ActivityLevel);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        isTrue = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Error Logging that a problem occurred, don't show the user
                throw new Exception("Review was not saved. Please try again.");
            }
            return isTrue;
        }

        private Park MapRowToParks(SqlDataReader reader)
        {
            return new Park()
            {
                ParkCode = Convert.ToString(reader["parkCode"]),
                Name = Convert.ToString(reader["parkName"]),
                State = Convert.ToString(reader["state"]),
                Acreage = Convert.ToInt32(reader["acreage"]),
                ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]),
                MilesOfTrail = Convert.ToInt32(reader["acreage"]),
                NumberOfCampsites = Convert.ToInt32(reader["acreage"]),
                Climate = Convert.ToString(reader["climate"]),
                YearFounded = Convert.ToInt32(reader["yearFounded"]),
                AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]),
                InspirQuote = Convert.ToString(reader["inspirationalQuote"]),
                InspirQSource = Convert.ToString(reader["inspirationalQuoteSource"]),
                Description = Convert.ToString(reader["parkDescription"]),
                EntryFee = Convert.ToInt32(reader["entryFee"]),
                NumAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]),

            };
        }

        private Weather MapRowToWeather(SqlDataReader reader)
        {
            return new Weather()
            {
                ParkCode = Convert.ToString(reader["parkCode"]),
                ForecastValue = Convert.ToInt32(reader["fiveDayForecastValue"]),
                LowTemp = Convert.ToInt32(reader["low"]),
                HighTemp = Convert.ToInt32(reader["high"]),
                Forecast = Convert.ToString(reader["forecast"])
            };
        }

        private Survey MapRowToSurvey(SqlDataReader reader)
        {
            return new Survey()
            {
                SurveyId = Convert.ToInt32(reader["surveyId"]),
                ParkCode = Convert.ToString(reader["parkCode"]),
                Email = Convert.ToString(reader["emailAddress"]),
                State = Convert.ToString(reader["state"]),
                ActivityLevel = Convert.ToString(reader["activityLevel"]),
            };
        }

        private Survey MapRowToSurveyCount(SqlDataReader reader)
        {
            return new Survey()
            {
                SurveyCount = Convert.ToInt32(reader["numSurveys"]),
                ParkCode = Convert.ToString(reader["parkCode"]),
                ParkName = Convert.ToString(reader["parkName"]),
            };
        }
        
    }
}