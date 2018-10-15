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

namespace Subscriber.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class Topic2Controller : ControllerBase
    {        
        [HttpPost] //<-- /webhook/topic1
        public ActionResult Post()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var jsonContent = reader.ReadToEnd();

                // Check the event type.
                // Return the validation code if it's a "SubscriptionValidation" request. 
                // Act on the Notification otherwise
                if (HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() =="SubscriptionValidation")
                {
                    return HandleValidation(jsonContent);
                }
                else if (HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() == "Notification")
                {
                    // Check to see if this is passed in using
                    // the CloudEvents schema
                    if (IsCloudEvent(jsonContent))
                    {
                        return HandleCloudEvent(jsonContent);
                    }

                    return HandleGridEvents(jsonContent);
                }

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
        private ActionResult HandleGridEvents(string jsonContent)
        {
            var events = JArray.Parse(jsonContent);
            foreach (var e in events)
            {
                // Invoke a method on the clients for 
                // an event grid notiification.                        
                var details = JsonConvert.DeserializeObject<GridEvent<dynamic>>(e.ToString());
            }

            return Ok();
        }
        private ActionResult HandleCloudEvent(string jsonContent)
        {
            var details = JsonConvert.DeserializeObject<CloudEvent<dynamic>>(jsonContent);

            // CloudEvents schema and mapping to 
            // Event Grid: https://docs.microsoft.com/en-us/azure/event-grid/cloudevents-schema 

            return Ok();
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
                if (!string.IsNullOrEmpty(version)) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
