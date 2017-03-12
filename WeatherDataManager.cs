using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2
{
    public class WeatherDataManager
    {
        private const string WeatherHistoryTable = "weatherhistory";

        private const string WeatherCityTable = "weathercityn";

        public void UpdateHistoryInfoToTable(WeatherInfoEntity model)
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CreateBlobTableAndUploadWeatherHistoryInfo(storageAccount, model);
        }

        public void UpdateCityInfoToTable(string city, string date)
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CityInfoEntity weatherInfo = new CityInfoEntity(city, date);
            CreateBlobTableAndUploaCityInfo(storageAccount, weatherInfo);
        }

        public void DeleteWeatherInfo(string city, string date)
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(WeatherHistoryTable);
            //create the table if it doesn't exist.
            table.CreateIfNotExists();

            TableQuery<WeatherInfoEntity> weatherInfosQuery = new TableQuery<WeatherInfoEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, city));

            List<WeatherInfoEntity> weatherInfoModels = new List<WeatherInfoEntity>();
            foreach (WeatherInfoEntity weatherInfo in table.ExecuteQuery(weatherInfosQuery))
            {
                if(weatherInfo.Date.Equals(date))
                {
                    table.Execute(TableOperation.Delete(weatherInfo));
                }
            }
        }

        public bool IsCityInMonitorList(string city)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(WeatherCityTable);
            //create the table if it doesn't exist.
            table.CreateIfNotExists();
            TableQuery<CityInfoEntity> getcities = new TableQuery<CityInfoEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, city));
            IEnumerable<CityInfoEntity> cites = table.ExecuteQuery(getcities);
            return cites.Count() == 1;
        }

        public string[] GetAllCities()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(WeatherCityTable);

            //table.DeleteIfExists();
            //create the table if it doesn't exist.
            table.CreateIfNotExists();
            List<string> cities = new List<string>();
            foreach(CityInfoEntity city in table.ExecuteQuery(new TableQuery<CityInfoEntity>()))
            {
                cities.Add(city.City);
            }
            return cities.ToArray();
        }

        public List<WeatherInfoEntity> GetWeatherHistory(string city)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(WeatherHistoryTable);
            //create the table if it doesn't exist.
            table.CreateIfNotExists();

            TableQuery<WeatherInfoEntity> weatherInfosQuery = new TableQuery<WeatherInfoEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, city));

            List<WeatherInfoEntity> weatherInfoModels = new List<WeatherInfoEntity>();
            foreach(WeatherInfoEntity weatherInfo in table.ExecuteQuery(weatherInfosQuery))
            {
                weatherInfoModels.Add(weatherInfo);
            }

            return weatherInfoModels;
        }

        private void CreateBlobTableAndUploadWeatherHistoryInfo(CloudStorageAccount storageAccount, WeatherInfoEntity weatherInfo)
        {
            //Create the table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Retrieve a reference to the table
            CloudTable table = tableClient.GetTableReference(WeatherHistoryTable);

            //create the table if it doesn't exist.
            table.CreateIfNotExists();

            //Create the tableOperation object that inserts the customer entity.
            //If the entry does exist, replace it
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(weatherInfo);
            table.Execute(insertOrReplaceOperation);
            
        }

        private void CreateBlobTableAndUploaCityInfo(CloudStorageAccount storageAccount, CityInfoEntity entity)
        {
            //Create the table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Retrieve a reference to the table
            CloudTable table = tableClient.GetTableReference(WeatherCityTable);

            //create the table if it doesn't exist.
            table.CreateIfNotExists();

            //Create the tableOperation object that inserts the customer entity.
            //If the entry does exist, replace it
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            table.Execute(insertOrReplaceOperation);
        }
    }
}