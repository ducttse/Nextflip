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
    [ApiController]
    public class ViewSupporterDashboard : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly string TECHNICAL_EMAIL = "technical.nextflipcompany@gmail.com";
        private readonly string CUSTOMER_RELATION_EMAIL = "customer_relation.nextflipcompany@gmail.com";
        public ViewSupporterDashboard(ILogger<ViewSupporterDashboard> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        [Route("GetPendingSupportTickets")]
        public IActionResult GetPendingSupportTickets([FromServices] ISupportTicketDAO supportTicketDAO, [FromBody] Request request)
        {
            try
            {
                int limit = request.RowsOnPage * request.NumberOfPage;
                int offset = (int) (request.RequestPage / request.NumberOfPage) * limit;

                IList<SupportTicket> pendingSupportTickets = supportTicketDAO.ViewPendingSupportTickets(limit, offset);
                return new JsonResult(pendingSupportTickets);
            }
            catch (Exception e)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetPendingSupportTickets: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }
        [Route("Respond")]
        [HttpPost]
        public async Task<IActionResult> Respond([FromServices] ISendMailService sendMailService, [FromServices] ISupportTicketDAO supportTicketDAO, [FromForm] string btnAction, [FromForm] string supportTicketID, [FromForm] string userEmail, [FromForm] string topicName, [FromForm] string content)
        {
            try
            {
                var mailContent = new MailContent();
                string toEmail = "nextflipcompany.com";
                content = "from user email: " + userEmail + "\n" + content;

                if (btnAction.Equals("technical")) toEmail = TECHNICAL_EMAIL;
                else if (btnAction.Equals("customerRelation")) toEmail = CUSTOMER_RELATION_EMAIL;
                await sendMailService.SendEmailAsync(toEmail, topicName, content);

                bool result = supportTicketDAO.ForwardSupportTicket(supportTicketID, toEmail).Result;
                if (result) return new JsonResult("Forward successful");
                return new JsonResult("An error occurred");
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
            catch (Exception e)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetSupportTicketDetails: " + e.Message);
                return new JsonResult(e.Message);
            }
        }
        [Route("GetNumOfSupportTicket")]
        public IActionResult GetNumOfSupportTicket([FromServices] ISupportTicketService supportTicketService)
        {
            try
            {
                int result = supportTicketService.GetNumOfSupportTickets();
                return new JsonResult(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetNumOfSupportTicket: " + e.Message);
                return new JsonResult(e.Message);
            }
        }

    }

    public class Request
    {
        public int NumberOfPage { get; set; }
        public int RowsOnPage { get; set; }
        public int RequestPage { get; set; }
    }
}
