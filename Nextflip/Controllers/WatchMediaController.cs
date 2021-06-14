﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Nextflip.Controllers
{
    public class WatchMediaController : Controller
    {
        [HttpGet("WatchMedia/Watch/{id}/{episodeID}")]
        public IActionResult Watch(string id, string episodeID)
        {
            ViewBag.MediaID = id;
            ViewBag.EpisodeID = episodeID;
            return View();
        }

        public IActionResult MediaDetails(string id)
        {
            ViewBag.MediaID = id;
            return View();
        }
    }
}