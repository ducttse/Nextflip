﻿using Microsoft.AspNetCore.Mvc;

namespace Nextflip.Controllers
{
    public class WatchMediaController : Controller
    {

        public WatchMediaController()
        {
        }

        [HttpPost]
        public IActionResult Watch([FromForm] string episodeUrl)
        {
            ViewBag.episodeUrl = episodeUrl;
            return View();
        }
    }
}