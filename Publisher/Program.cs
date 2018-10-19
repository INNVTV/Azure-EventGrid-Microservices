using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Publisher.Models;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generate AppSettings
            Settings.ApplyAppSettings();

            // Connect to our EventGrid topics
            var topic1Credentials = new TopicCredentials(AppSettings.Topic1.Key);
            var topic2Credentials = new TopicCredentials(AppSettings.Topic2.Key);

            var eventClient_Topic1 = new EventGridClient(topic1Credentials);
            var eventClient_Topic2 = new EventGridClient(topic2Credentials);

            // Create our Event data:
            var eventsListTopic1 = GetTopic1_Events();
            var eventsListTopic2 = GetTopic2_Events();   

            Console.WriteLine(eventsListTopic1[0].EventType);  

            while(true)
            {
                // Send a "TopicOne" event every 10 seconds
                // and a "TopicTwo" event every 20 seconds  
                Console.WriteLine("Sending 'TopicOne' event to the grid...");
                eventClient_Topic1.PublishEventsAsync(new Uri(AppSettings.Topic1.Endpoint).Host, eventsListTopic1).GetAwaiter().GetResult();
                Thread.Sleep(10000);
                
                Console.WriteLine("Sending 'TopicOne' event to the grid...");
                Console.WriteLine("Sending 'TopicTwo' event to the grid...");
                eventClient_Topic1.PublishEventsAsync(new Uri(AppSettings.Topic1.Endpoint).Host, eventsListTopic1).GetAwaiter().GetResult();
                eventClient_Topic2.PublishEventsAsync(new Uri(AppSettings.Topic2.Endpoint).Host, eventsListTopic2).GetAwaiter().GetResult();
                Thread.Sleep(10000);
            }
        }

        private static List<EventGridEvent> GetTopic1_Events()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Notification",
                    Data = new CustomTopicData()
                    {
                        Topic = "Topic1",
                        Source = "Publisher"
                    },

                    EventTime = DateTime.Now,
                    Subject = "Topic1",
                    DataVersion = "2.0"
                });

                return eventsList;
        }

        private static List<EventGridEvent> GetTopic2_Events()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Notification",
                    Data = new CustomTopicData()
                    {
                        Topic = "Topic2",
                        Source = "Publisher"
                    },

                    EventTime = DateTime.Now,
                    Subject = "Topic2",
                    DataVersion = "2.0"
                });
                eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Notification",
                    Data = new CustomTopicData()
                    {
                        Topic = "Topic2",
                        Source = "Publisher"
                    },

                    EventTime = DateTime.Now,
                    Subject = "Topic2",
                    DataVersion = "2.0"
                });

                return eventsList;
        }
    }
}
