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
        [HttpGet("/MediaManagerManagement/DetailPreview/{type}/{id}/{requestid}")]
        public IActionResult DetailPreview(string type,string id, string requestid)
        {
            ViewBag.Type = type;
            ViewBag.MediaID = id;
            ViewBag.RequestID = requestid;
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
    }
}
