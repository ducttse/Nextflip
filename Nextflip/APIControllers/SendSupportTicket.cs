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
        public IActionResult SendTicket([FromServices] ISupportTicketService supportTicketService, [FromBody] SupportTicketDetails request)
        {
            try
            {
                bool result = supportTicketService.SendSupportTicket(request.UserEmail, request.TopicName, request.Content);
                if (result) return new JsonResult("Success");
                return new JsonResult("An error occurred");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SendSupportTicket/SendTicket: " + ex.Message);
                return new JsonResult("An error occurred");
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
