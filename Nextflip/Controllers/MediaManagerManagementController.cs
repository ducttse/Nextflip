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
        public IActionResult DetailPreview(string id)
        {
            ViewBag.CategoryID = id;
            return View();
        }
    }
}
