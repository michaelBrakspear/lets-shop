using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace LetsShop.Basket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadyController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult(DateTime.Now);
        }
    }
}
