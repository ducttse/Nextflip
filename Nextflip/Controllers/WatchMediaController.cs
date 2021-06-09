using Microsoft.AspNetCore.Mvc;

namespace Nextflip.Controllers
{
    public class WatchMediaController : Controller
    {
        public IActionResult Watch()
        {
            return View("Watch");
        } 
    }
}