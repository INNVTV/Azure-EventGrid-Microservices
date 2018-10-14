using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Subscriber.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class TopicTwoController : ControllerBase
    {
        // GET api/topicTwo
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Topic two event notification received!" };
        }
    }
}
