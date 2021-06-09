using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Services.Interfaces;
using System;

namespace Nextflip.Controllers
{
    public class SendingSupportTicket : Controller
    {
        private readonly ILogger _logger;
        public SendingSupportTicket(ILogger logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
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
                _logger.LogInformation("SendingSupportTicket: " + e.Message);
                return View("Error");
            }
        }
    }
}
