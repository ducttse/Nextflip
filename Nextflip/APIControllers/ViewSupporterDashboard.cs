using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.supportTicket;
using Nextflip.Services.Interfaces;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    public class ViewSupporterDashboard : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly string TECHNICAL_EMAIL = "technical.nextflipcompany@gmail.com";
        private readonly string CUSTOMER_RELATION_EMAIL = "customer_relation.nextflipcompany@gmail.com";
        public ViewSupporterDashboard(ILogger<ViewSupporterDashboard> logger)
        {
            _logger = logger;
        }

        [Route("GetPendingSupportTickets")]
        public IActionResult GetPendingSupportTickets([FromServices] ISupportTicketDAO supportTicketDAO)
        {
            try {
                IList<SupportTicket> pendingSupportTickets = supportTicketDAO.ViewAllPendingSupportTickets();
                return new JsonResult(pendingSupportTickets);
            }
            catch (Exception e)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetPendingSupportTickets: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }
        [Route("Respond")]
        public async Task<IActionResult> Respond([FromServices] ISendMailService sendMailService, [FromServices] ISupportTicketDAO supportTicketDAO, [FromBody] string btnAction, [FromBody] string supportTicketID, [FromBody] string userEmail, [FromBody] string topicName, [FromBody] string content)
        {
            try {
                bool result = false;
                var mailContent = new MailContent();
                string toEmail = "nextflipcompany.com";
                content = "From userEmail: " + userEmail + "\n" + content;

                if (btnAction.Equals("technical")) toEmail = TECHNICAL_EMAIL;
                else if (btnAction.Equals("customerRelation")) toEmail = CUSTOMER_RELATION_EMAIL;
                await sendMailService.SendEmailAsync(toEmail, topicName, content);

                result = supportTicketDAO.ForwardSupportTicket(supportTicketID, toEmail).Result;
                return new JsonResult("Forward successful");
            }
            catch (Exception e)
            {
                _logger.LogInformation("ViewSupporterDashboard/Respond: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }
        [Route("GetSupportTicketDetails/{supportTicketID}")]
        public IActionResult GetSupportTicketDetails([FromServices] ISupportTicketService supportTicketService, string supportTicketID)
        {
            try
            {
                SupportTicket supportTicket = supportTicketService.ViewSupportTicketByID(supportTicketID);
                return new JsonResult(supportTicket);
            }
            catch(Exception e)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetSupportTicketDetails: " + e.Message);
                return new JsonResult(e.Message);
            }
        }

    }
}
