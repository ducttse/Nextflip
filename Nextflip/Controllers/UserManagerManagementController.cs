using Microsoft.AspNetCore.Mvc;
using Nextflip.Models.account;
using Nextflip.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nextflip.Controllers
{
    [Authorize(Policy = "user manager")]

    public class UserManagerManagementController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateStaff()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditExpiredDate(string id)
        {
            ViewBag.UserID = id;
            return View();
        }

        [HttpPost]
        public ActionResult EditStaffProfile(string id)
        {
            ViewBag.UserID = id;
            return View();
        }
        public ActionResult ViewSubscriptionManage()
        {
            return View();
        }
    }
}
