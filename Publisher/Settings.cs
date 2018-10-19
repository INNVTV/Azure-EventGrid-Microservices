using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Publisher.Models;

namespace Publisher
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

            var _topic1Endpoint = configuration["Topic1:Endpoint"];
            var _topic1Key = configuration["Topic1:Key"];

            var _topic2Endpoint = configuration["Topic2:Endpoint"];
            var _topic2Key = configuration["Topic2:Key"];

            AppSettings.Topic1.Endpoint = _topic1Endpoint;
            AppSettings.Topic1.Key = _topic1Key;

            AppSettings.Topic2.Endpoint = _topic2Endpoint;
            AppSettings.Topic2.Key = _topic2Key;
        }
    }
}