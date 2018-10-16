using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Validator.Models;

namespace Validator
{
    public static class Settings
    {
        public static AppSettings GetAppSettings()
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

            var appSettings = new AppSettings{
                    storageName = _storageName,
                    storageKey = _storageKey,
                    queueName = _queueName,
                    queueAddress = string.Concat("https://", _storageName, ".queue.core.windows.net/", _queueName),
                    connectionString = string.Concat("DefaultEndpointsProtocol=https;AccountName=", _storageName ,";AccountKey=", _storageKey)
                };

            return appSettings;
        }
    }
}