using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nextflip.Controllers
{
    [Authorize(Policy = "media editor")]

    public class EditorDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewEditRequest()
        {
            return View();
        }
        public IActionResult ViewAddNewMedia()
        {
            return View();
        }
    }
}
