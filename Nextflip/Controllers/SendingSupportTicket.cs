using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Services.Interfaces;
using System;

namespace Nextflip.Controllers
{
    public class SendingSupportTicket : Controller
    {
        private readonly ILogger _logger;
        public SendingSupportTicket(ILogger<SendingSupportTicket> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            try {
                return View();
            }
            catch(Exception e)
            {
                _logger.LogInformation("SendingSupportTicket/Index: " + e.Message);
                return View("Error");
            }
        }
        [HttpPost]
        public IActionResult Send([FromServices] ISupportTicketService supportTicketService, [FromForm] string userEmail, [FromForm]string topicName, [FromForm]string content)
        {
            try {
                bool result = supportTicketService.SendSupportTicket(userEmail, topicName, content);
                return View();
            }
            catch(Exception e)
            {
                _logger.LogInformation("SendingSupportTicket/Send: " + e.Message);
                return View("Error");
            }
        }
    }
}
