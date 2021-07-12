using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
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
