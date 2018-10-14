using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Subscriber.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class Topic1Controller : ControllerBase
    {
        // GET api/topic1
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Topic 1 event notification received!" };
        }
    }
}
