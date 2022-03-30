using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace yingwebappdemo
{
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [Route("/")]
        [HttpGet]
        public string Get()
        {
            Console.WriteLine($"{DateTime.UtcNow}:receive a request");
            System.Threading.Thread.Sleep(30 * 1000);
            Console.WriteLine($"{DateTime.UtcNow}:end of a request");
            return "ok";
        }

    }
}
