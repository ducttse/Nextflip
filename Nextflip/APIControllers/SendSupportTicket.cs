using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.supportTopic;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    public class SendSupportTicket : ControllerBase
    {
        private partial class TicketError
        {
            public string UserEmailError { get; set; }
            public string TopicError { get; set; }
            public string ContentError { get; set; }
        }

        private readonly ILogger _logger;
        public SendSupportTicket(ILogger<SendSupportTicket> logger)
        {
            _logger = logger;
        }

        [Route("GetAllTopic")]
        public IActionResult GetAllTopic([FromServices]ISupportTopicService supportTopicService)
        {
            try
            {
                IList<SupportTopic> supportTopics = supportTopicService.GetAllTopics();
                return new JsonResult(supportTopics);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SendSupportTicket/GetAllTopic: " + ex.Message);
                return new JsonResult("An error occurred");
            }
        }
        [Route("SendTicket")]
        [HttpPost]
        public IActionResult SendTicket([FromServices] ISupportTicketService supportTicketService, [FromServices] IAccountService accountService,[FromBody] SupportTicketDetails request)
        {
            try
            {
                TicketError error = new TicketError();
                bool isValid = true;
                if (request.UserEmail == null || request.UserEmail.Trim().Length == 0 || !accountService.IsExistedEmail(request.UserEmail))
                {
                    isValid = false;
                    error.UserEmailError = "Invalid email !";
                }
                if(request.TopicName == null || request.Content.Trim().Length == 0)
                {
                    isValid = false;
                    error.TopicError = "Please choose a ticket topic !";
                }
                if(request.Content == null || request.Content.Trim().Length == 0)
                {
                    isValid = false;
                    error.ContentError = "Please enter your issue !";
                    
                }
                if (!isValid) return new JsonResult(error);
                bool result = supportTicketService.SendSupportTicket(request.UserEmail, request.TopicName, request.Content);
                if (!result) throw new Exception("SendSupportTicket invalid result");
                return new JsonResult(new { Message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SendSupportTicket/SendTicket: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }
    }

    public class SupportTicketDetails
    {
        public string UserEmail { get; set; }
        public string TopicName { get; set; }
        public string Content { get; set; }
    }
}
