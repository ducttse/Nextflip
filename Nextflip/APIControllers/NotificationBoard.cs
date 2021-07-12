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

        [HttpPost]
        [Route("ViewAllNotifications")]
        public IActionResult ViewAllNotifications([FromServices] INotificationService notificationService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Notification> notifications = notificationService.ViewAllNotifications(request.status, request.RowsOnPage, request.RequestPage);
                int count = notificationService.CountNotification(request.status);
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

        public class Request
        {
            public string status { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        } 

        [HttpPost]
        [Route("ViewAvailableNotifications")]
        public IActionResult ViewAvailableNotifications([FromServices] INotificationService notificationService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Notification> notifications = notificationService.ViewAvailableNotifications(request.RowsOnPage, request.RequestPage);
                int count = notificationService.CountAvailableNotification();
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
                _logger.LogInformation("GetAvailableNotificationsList: " + e.Message);
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


        [Route("AddNotification")]
        public IActionResult AddNotification([FromServices] INotificationService notificationService, [FromForm] Notification notificationAdding)
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

        [Route("EditNotification")]
        public IActionResult EditNotification([FromServices] INotificationService notificationService, [FromForm] Notification notification)
        {
            try
            {
                bool result = notificationService.EditNotification(notification.notificationID , notification.title, notification.content, notification.status);
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

        /*[Route("CountNotification")]
        public IActionResult CountNotification([FromServices] INotificationService notificationService)
        {
            int count = notificationService.CountNotification();
            return new JsonResult(count);
        }*/
    }
}
