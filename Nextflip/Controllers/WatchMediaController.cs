using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    public class WatchMediaController : Controller
    {

        public WatchMediaController()
        {
        }

        public IActionResult Watch([FromBody] string episodeUrl)
        {
            ViewBag.episodeUrl = episodeUrl;
            return View();
        }
    }
}