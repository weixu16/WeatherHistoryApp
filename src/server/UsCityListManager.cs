using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    public class UsCityListManager
    {
        private CloudStorageAccount cloudStorageAccount; 

        public UsCityListManager()
        {
            this.cloudStorageAccount = this.CreateStorageAccount();
            CloudBlobClient blobClient = this.cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("uscity");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("us_cities_states_counties.csv");

        }

        public string[] getCityNameFromPrefix(string prefix)
        {
            return new string[] { };
        }

        private CloudStorageAccount CreateStorageAccount()
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            return storageAccount;
        }
    }
}