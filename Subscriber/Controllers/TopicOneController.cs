using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Subscriber.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class TopicOneController : ControllerBase
    {
        // GET api/topicOne
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Topic one event notification received!" };
        }
    }
}
