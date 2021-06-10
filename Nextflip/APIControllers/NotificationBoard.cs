using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.notification;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationBoard : ControllerBase
    {
        [Route("GetAllNotifications")]
        public IActionResult GetAllNotifications([FromServices] INotificationService notificationService)
        {
            IEnumerable<Notification> notifications = notificationService.GetAllNotifications();
            return new JsonResult(notifications);
        }

        [Route("GetNotificationsWithPaging/{pageNum}")]
        public IActionResult GetNotificationsWithPaging([FromServices] INotificationService notificationService, int pageNum)
        {
            IEnumerable<Notification> notifications = notificationService.GetNotificationsWithPaging(pageNum);
            return new JsonResult(notifications);
        }

        [Route("GetDetailOfNotification/{notificationID}")]
        public IActionResult GetDetailOfNotification([FromServices] INotificationService notificationService, int notificationID)
        {
            Notification notification = notificationService.GetDetailOfNotification(notificationID);
            return new JsonResult(notification);
        }

        [Route("GetAllAvailableNotifications")]
        public IActionResult GetAllAvailableNotifications([FromServices] INotificationService notificationService)
        {
            IEnumerable<Notification> notifications = notificationService.GetAllAvailableNotifications();
            return new JsonResult(notifications);
        }

        /*[Route("AddNotification")]
        public IActionResult AddNotification([FromServices] INotificationService notificationService, string title, string content)
        {
            bool result = notificationService.AddNotification(title, content);
            return new JsonResult(result);
        } */

        [Route("CountNotification")]
        public IActionResult CountNotification([FromServices] INotificationService notificationService)
        {
            int count = notificationService.CountNotification();
            return new JsonResult(count);
        }
    }
}
