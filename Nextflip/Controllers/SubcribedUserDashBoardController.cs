﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
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
    }
}
