using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Nextflip.Controllers
{
    public class WatchMediaController : Controller
    {
        
        public IActionResult Watch(string id)
        {
            ViewBag.EpisodeID = id;
            return View();
        }

        public IActionResult MediaDetails(string id)
        {
            ViewBag.MediaID = id;
            return View();
        }
    }
}