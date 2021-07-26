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
        [HttpGet("/MediaManagerManagement/DetailPreview/{id}")]
        public IActionResult DetailPreview(string id)
        {
            ViewBag.MediaID = id;
            return View();
        }
        public IActionResult CategoryManager()
        {
            return View();
        }
        public IActionResult MediaTypeManager()
        {
            return View();
        }
        [HttpGet("/MediaManagerManagement/PreviewEpisode/{id}")]
        public IActionResult PreviewEpisode(string id)
        {
            ViewBag.MediaID = id;
            return View();
        }
        public ActionResult ViewNotificationManage()
        {
            return View();
        }
    }
}
