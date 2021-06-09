using Microsoft.AspNetCore.Mvc;

namespace Nextflip.Controllers
{
    public class WatchMediaController : Controller
    {
        [Route("{episodeID}")]
        public IActionResult Watch(string episodeID)
        {
            ViewBag.EpisodeID = episodeID;
            return View();
        } 

        public IActionResult MediaDetails(string mediaID)
        {
            ViewBag.MediaID = mediaID;
            return View();
        }
    }
}