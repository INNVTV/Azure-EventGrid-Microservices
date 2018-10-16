using System;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Queue;

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
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(appSettings.connectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(appSettings.queueName);
            queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue.
            //CloudQueueMessage message = new CloudQueueMessage("Hello, World");
            //queue.AddMessage(message);




            while(true)
            {
                // Check message queue every 60 seconds...
                Console.WriteLine("Checking message queue...");

                // Get a batch of 10 messages
                List<CloudQueueMessage> retrievedMessages = queue.GetMessagesAsync(10).Result.ToList();
     
                if(retrievedMessages.Count > 0)
                {
                    Console.WriteLine(string.Concat("Got ", retrievedMessages.Count, " messages:"));

                    //Process the message in less than 30 seconds, and then delete each one:
                    foreach(var message in retrievedMessages)
                    {
                        Console.WriteLine(string.Concat("Message: ", message.AsString));
                        queue.DeleteMessageAsync(message);
                    }     
                }
                else{
                    Console.WriteLine("Queue empty.");
                }
                
                Console.WriteLine("Sleeping for 1 minute...");
                Thread.Sleep(60000);
            }
        }
    }
}
