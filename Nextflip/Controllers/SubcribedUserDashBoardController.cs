using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nextflip.Controllers
{
    [Authorize(Policy = "subscribed user")]
    public class SubcribedUserDashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string id)
        {
            ViewBag.SearchValue = id;
            return View();
        }
        public IActionResult Profile(string id)
        {
            ViewBag.UserID = id;
            return View();
        }

        public IActionResult ViewByCategory(string id)
        {
            ViewBag.CategoryID = id;
            return View();
        }

        public IActionResult ViewFavourite()
        {
            return View();
        }
    }
}
