using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    public class MediaManagerManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/MediaManagerManagement/DetailPreview/{id}/{requestid}")]
        public IActionResult DetailPreview(string id, string requestid)
        {
            ViewBag.MediaID = id;
            ViewBag.RequestID = requestid;
            return View();
        }
    }
}
