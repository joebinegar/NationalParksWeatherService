using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string ParkCode { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string State { get; set; }
        public string ActivityLevel { get; set; }
        public int SurveyCount { get; set; }
        public string ParkName { get; set; }


    }
}