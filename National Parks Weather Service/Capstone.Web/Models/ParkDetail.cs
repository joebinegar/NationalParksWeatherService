using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class ParkDetail
    {
        
            public string ParkCode { get; set; }
            public string Name { get; set; }
            public string State { get; set; }
            public int Acreage { get; set; }
            public int ElevationInFeet { get; set; }
            public int MilesOfTrail { get; set; }
            public int NumberOfCampsites { get; set; }
            public string Climate { get; set; }
            public int YearFounded { get; set; }
            public int AnnualVisitorCount { get; set; }
            public string InspirQuote { get; set; }
            public string InspirQSource { get; set; }
            public string Description { get; set; }
            public int EntryFee { get; set; }
            public int NumAnimalSpecies { get; set; }

        public Weather weather { get; set; }

        public List<Weather> Forecast { get; set; }

        public void PopulateParkProperties(Park park)
        {
            ParkCode = park.ParkCode;
            Name = park.Name;
            State = park.State;
            Acreage = park.Acreage;
            ElevationInFeet = park.ElevationInFeet;
            MilesOfTrail = park.MilesOfTrail;
            NumberOfCampsites = park.NumberOfCampsites;
            Climate = park.Climate;
            YearFounded = park.YearFounded;
            AnnualVisitorCount = park.AnnualVisitorCount;
            InspirQuote = park.InspirQuote;
            InspirQSource = park.InspirQSource;
            Description = park.Description;
            EntryFee = park.EntryFee;
            NumAnimalSpecies = park.NumAnimalSpecies;
        }

        public void PopulateForecast(List<Weather>forecast)
        {
            Forecast = forecast;
        }
    }
}