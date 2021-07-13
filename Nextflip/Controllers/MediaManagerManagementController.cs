using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nextflip.Controllers
{
    [Authorize(Policy = "media manager")]

    public class MediaManagerManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
