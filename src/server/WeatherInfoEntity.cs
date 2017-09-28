using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2
{
    public class WeatherInfoEntity : TableEntity
    {
        public WeatherInfoEntity(string city, double highTemp, double lowTemp, string date)
        {
            this.PartitionKey = city;
            this.RowKey = date.Replace('/', ' ');
            this.City = city;
            this.HighTemp = highTemp;
            this.LowTemp = lowTemp;
            this.Date = date;
        }

        public WeatherInfoEntity() { }

        public string City { get; set; }

        public double HighTemp { get; set; }

        public double LowTemp { get; set; }

        public string Date { get; set; }
    }
}