using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.notification;
using Microsoft.Extensions.Logging;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationBoard : ControllerBase
    {
        private readonly ILogger _logger;

        public NotificationBoard(ILogger<NotificationBoard> logger)
        {
            _logger = logger;
        }

        [Route("GetAllNotifications")]
        public IActionResult GetAllNotifications([FromServices] INotificationService notificationService)
        {
            try
            {
                IEnumerable<Notification> notifications = notificationService.GetAllNotifications();
                return new JsonResult(notifications);
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetAllNotifications: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        public class Request
        {
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        } 

        [HttpPost]
        [Route("GetNotificationsListAccordingRequest")]
        public IActionResult GetNotificationsListAccordingRequest([FromServices] INotificationService notificationService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Notification> notifications = notificationService.GetNotificationsListAccordingRequest(request.RowsOnPage, request.RequestPage);
                int count = notificationService.CountNotification();
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = notifications
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetNotificationsListAccordingRequest: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        [Route("GetDetailOfNotification/{notificationID}")]
        public IActionResult GetDetailOfNotification([FromServices] INotificationService notificationService, int notificationID)
        {
            try
            {
                Notification notification = notificationService.GetDetailOfNotification(notificationID);
                return new JsonResult(notification);
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetDetailOfNotification: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        [Route("GetAllAvailableNotifications")]
        public IActionResult GetAllAvailableNotifications([FromServices] INotificationService notificationService)
        {
            try
            {
                IEnumerable<Notification> notifications = notificationService.GetAllAvailableNotifications();
                return new JsonResult(notifications);
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetAllAvailableNotifications: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        public class NotificationAdding
        {
            public string title { get; set; }
            public string content { get; set; }
        }

        [Route("AddNotification")]
        public IActionResult AddNotification([FromServices] INotificationService notificationService, [FromForm] NotificationAdding notificationAdding)
        {
            try 
            { 
                bool result = notificationService.AddNotification(notificationAdding.title, notificationAdding.content);
                var message = new
                    {
                        message = "success"
                    };
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                _logger.LogInformation("AddNotification: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("CountNotification")]
        public IActionResult CountNotification([FromServices] INotificationService notificationService)
        {
            int count = notificationService.CountNotification();
            return new JsonResult(count);
        }
    }
}
