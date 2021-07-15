using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nextflip.Controllers
{
    [Authorize(Policy = "subscribed user")]
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetDetailOfNotification(int notificationID)
        {
            ViewBag.NotificationID = notificationID;
            return View();
        }
    }
}
