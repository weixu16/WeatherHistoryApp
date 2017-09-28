using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class WeatherHistoryModel
    {
        public string City { get; set; }
        
        public double[] HighTemp { get; set; }

        public double[] LowTemp { get; set; }

        public string[] Date { get; set; }
    }
}