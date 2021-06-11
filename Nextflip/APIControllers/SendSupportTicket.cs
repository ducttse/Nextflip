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
            catch (Exception e)
            {
                _logger.LogInformation("SendSupportTicket/GetAllTopic: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }
    }
}
