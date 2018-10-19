using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Subscriber.Models;

namespace Subscriber
{
    public static class Settings
    {
        public static void ApplyAppSettings()
        {
            //Get configuration from Docker/Compose (via .env and appsettings.json)
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(); //<-- Allows for Docker Env Variables

            IConfigurationRoot configuration = builder.Build();

            var _storageName = configuration["Storage:Name"];
            var _queueName = configuration["Storage:Queue"];
            var _storageKey = configuration["Storage:Key"];

            AppSettings.StorageName = _storageName;
            AppSettings.StorageKey = _storageKey;
            AppSettings.QueueName = _queueName;
            AppSettings.QueueAddress = string.Concat("https://", _storageName, ".queue.core.windows.net/", _queueName);
            AppSettings.ConnectionString = string.Concat("DefaultEndpointsProtocol=https;AccountName=", _storageName ,";AccountKey=", _storageKey);
        }
    }
}