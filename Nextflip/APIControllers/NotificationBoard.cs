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
            public string SearchValue { get; set; }
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
                    TotalPage = (int)Math.Ceiling(totalPage),
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
        public IActionResult AddNotification([FromServices] INotificationService notificationService, [FromBody] Notification notificationAdding)
        {
            if (notificationAdding.title.Trim().Length == 0) return new JsonResult(new { message = "fail. Title is required" });
            if (notificationAdding.content.Trim().Length == 0) return new JsonResult(new { message = "fail. Content is required" });
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
        public IActionResult EditNotification([FromServices] INotificationService notificationService, [FromBody] Notification notification)
        {
            try
            {
                if (notification.notificationID == 0) return new JsonResult(new { message = "fail. NotificationID is required" });
                if (notification.title.Trim().Length == 0) notification.title = notificationService.GetDetailOfNotification(notification.notificationID).title;
                if (notification.content.Trim().Length == 0) notification.content = notificationService.GetDetailOfNotification(notification.notificationID).content;
                if (notification.status.Trim().Length == 0) notification.status = notificationService.GetDetailOfNotification(notification.notificationID).status;
                bool result = notificationService.EditNotification(notification.notificationID , notification.title, notification.content, notification.status);
                var message = new
                {
                    message = "success"
                };
                if (result) return new JsonResult(message);
                else return new JsonResult(new {message = "fail" });
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

        [HttpPost]
        [Route("ViewNotifications")]
        public IActionResult ViewNotifications([FromServices] INotificationService notificationService,
            [FromBody] Request request)
        {
            if (request.status.Trim().Length == 0) request.status = "All";
            try
            {
                IEnumerable<Notification> notifications = notificationService.ViewAllNotifications(request.status, request.RowsOnPage, request.RequestPage);
                int count = notificationService.CountNotification(request.status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = notifications
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("ViewNotifications: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        [HttpPost]
        [Route("SearchNotifications")]
        public IActionResult SearchNotifications([FromServices] INotificationService notificationService,
            [FromBody] Request request)
        {
            if (request.SearchValue.Trim() == "") return new JsonResult(new { message = "Empty searchValue" });
            if (request.status.Trim().Length == 0) request.status = "All";
            try
            {
                IEnumerable<Notification> notifications = notificationService.SearchNotifications(request.SearchValue, request.status, request.RowsOnPage, request.RequestPage);
                int count = notificationService.CountSearchNotification(request.SearchValue, request.status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = notifications
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("SearchNotifications: " + e.Message);
                return new JsonResult("fail");
            }
        }
    }
}
