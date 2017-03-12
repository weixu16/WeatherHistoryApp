using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    public class CityInfoEntity : TableEntity
    {
        public CityInfoEntity(string city, string date)
        {
            this.PartitionKey = city;
            this.RowKey = " ";
            this.City = city;
            this.Date = date;
        }

        public CityInfoEntity() { }

        public string City { get; set; }
        public string Date { get; set; }
    }
}