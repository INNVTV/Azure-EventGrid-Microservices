using System;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Validator.Models;

namespace Validator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get Application Settings:
            var appSettings = Settings.GetAppSettings();

            // Connect to the
            // storage message queue:
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(appSettings.ConnectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(appSettings.QueueName);
            queue.CreateIfNotExistsAsync();


            while(true)
            {
                // Check message queue every 60 seconds...
                Console.WriteLine("Checking message queue...");

                // Get a batch of 20 messages
                List<CloudQueueMessage> retrievedMessages = queue.GetMessagesAsync(20).Result.ToList();
     
                if(retrievedMessages.Count > 0)
                {
                    Console.WriteLine(string.Concat("Got ", retrievedMessages.Count, " messages:"));

                    //Process the message in less than 30 seconds, and then delete each one:
                    foreach(var message in retrievedMessages)
                    {
                        var deserializedMessage = JsonConvert.DeserializeObject<QueueMessage>(message.AsString);
                        Console.WriteLine(string.Concat("Type: ", deserializedMessage.EventType));
                        Console.WriteLine(string.Concat("Topic: ", deserializedMessage.Topic));
                        Console.WriteLine(string.Concat("Source: ", deserializedMessage.Source));
                        Console.WriteLine(string.Concat("Count: ", deserializedMessage.EventCount));
                        Console.WriteLine();

                        //Delete message from queue:
                        queue.DeleteMessageAsync(message);
                    }     
                }
                else{
                    Console.WriteLine("Queue empty.");
                }

                Console.WriteLine("Sleeping for 30 seconds...");
                Thread.Sleep(30000);
            }
        }
    }
}
