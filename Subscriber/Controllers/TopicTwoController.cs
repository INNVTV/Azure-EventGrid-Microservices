using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Subscriber.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class Topic2Controller : ControllerBase
    {
        // GET api/topic2
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Topic 2 event notification received!" };
        }
    }
}
