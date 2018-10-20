using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Subscriber.Models;
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Queue;


namespace Subscriber.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class Topic1Controller : ControllerBase
    {  
        [HttpPost] //<-- /webhook/topic1
        public ActionResult Post()
        {
            // Connect to the
            // storage message queue:
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AppSettings.ConnectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(AppSettings.QueueName);
            queue.CreateIfNotExistsAsync();

            try{

            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var jsonContent = reader.ReadToEnd();

                var eventType = "";

                // Check the event type.
                // Return the validation code if it's a "SubscriptionValidation" request. 
                // Act on the Notification otherwise
                if (HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() =="SubscriptionValidation")
                {
                    return HandleValidation(jsonContent);
                }
                else if (HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() == "Notification") //SWITCH BACK
                {
                    var _topic = String.Empty;
                    var _source = String.Empty;
                    var _count = 0;

                    // Check to see if this is passed in using the CloudEvents schema
                    if (IsCloudEvent(jsonContent))
                    {
                        eventType= "Cloud";
                        var cloudEvent = UnpackCloudEvent(jsonContent);

                        var topicData = JsonConvert.DeserializeObject<CustomTopicData>(cloudEvent.Data);

                        _topic = topicData.Topic;
                        _source = topicData.Source;
                        _count = 1; //<-- Will always be 1 if CloudEvent
                    }
                    else
                    {
                        eventType = "Grid";
                        var gridEvents = UnpackGridEvents(jsonContent);
                        
                        var topicData = JsonConvert.DeserializeObject<CustomTopicData>(gridEvents[0].Data);

                        _topic = topicData.Topic;
                        _source = topicData.Source;
                        _count = gridEvents.Count;
                    }               

                    // Create a message and add it to the queue.
                    var queueMessage = new QueueMessage{
                        Topic = _topic,
                        Source = _source + "---" + HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault(), //REMOVE
                        EventType = eventType,
                        EventCount = _count
                    };

                    var messageAsJson = JsonConvert.SerializeObject(queueMessage);
                    CloudQueueMessage message = new CloudQueueMessage(messageAsJson);
                    queue.AddMessageAsync(message);
                    
                    return Ok();
                }

                return BadRequest();                
            }

            
            }
            catch(Exception e)
            {
                // Create a message and add it to the queue.
                    var queueMessage = new QueueMessage{
                        Topic = "EXCEPTIPN",
                        Source = "R" + "---",
                        EventType = e.Message,
                        EventCount = 0
                    };

                    var messageAsJson = JsonConvert.SerializeObject(queueMessage);
                    CloudQueueMessage message = new CloudQueueMessage(messageAsJson);
                    queue.AddMessageAsync(message);

                    return BadRequest(); 
            }
        }
        private JsonResult HandleValidation(string jsonContent)
        {
            var gridEvent = JsonConvert.DeserializeObject<List<GridEvent<Dictionary<string, string>>>>(jsonContent).First();

            //Send back the validationCode to Azure EventGrid to 
            var validationCode = gridEvent.Data["validationCode"];
            return new JsonResult(new
            {
                validationResponse = validationCode
            });
        }
        private List<GridEvent<dynamic>> UnpackGridEvents(string jsonContent)
        {
            //Note: GridEvents are an array 
            var events = JArray.Parse(jsonContent);

            var detailsList = new List<GridEvent<dynamic>>();
            foreach (var e in events)
            {
                // Invoke a method on the clients for 
                // an event grid notiification.                        
                var details = JsonConvert.DeserializeObject<GridEvent<dynamic>>(e.ToString());
                detailsList.Add(details);
            }

            return detailsList;
        }
        private CloudEvent<dynamic> UnpackCloudEvent(string jsonContent)
        {
            //Note: CloudEvents are an single items 
            var details = JsonConvert.DeserializeObject<CloudEvent<dynamic>>(jsonContent);
            return details;

            // CloudEvents schema and mapping to 
            // Event Grid: https://docs.microsoft.com/en-us/azure/event-grid/cloudevents-schema 
        }    
        private static bool IsCloudEvent(string jsonContent)
        {
            // Cloud events are sent one at a time, while Grid events
            // are sent in an array. As a result, the JObject.Parse will 
            // fail for Grid events. 
            try
            {
                // Attempt to read one JSON object. 
                var eventData = JObject.Parse(jsonContent);

                // Check for the cloud events version property.
                var version = eventData["cloudEventsVersion"].Value<string>();
                if (!string.IsNullOrEmpty(version))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
