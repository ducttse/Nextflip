using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Services.Interfaces;
using System;

namespace Nextflip.Controllers
{
    public class SendSupportTicket : Controller
    {
        private readonly ILogger _logger;
        public SendSupportTicket(ILogger<SendSupportTicket> logger)
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

        public IActionResult Send([FromServices] ISupportTicketService supportTicketService,  string userEmail, string topicName, string content)
        {
            try {
                bool result = supportTicketService.SendSupportTicket(userEmail, topicName, content);
                if(result) return RedirectToAction("Index");
                return View("Error");
            }
            catch(Exception e)
            {
                _logger.LogInformation("SendingSupportTicket/Send: " + e.Message);
                return View("Error");
            }
        }
    }
}
