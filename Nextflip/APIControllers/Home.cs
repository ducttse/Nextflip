using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
        public ActionResult Get()
        {
            return new JsonResult("abc");
        }
    }
}
