using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.supportTicket;
using Nextflip.Models.supportTopic;
using Nextflip.Services.Interfaces;
using Nextflip.utils;
using System;
using System.Collections;
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
        private readonly string CUSTOMER_RELATION_EMAIL = "customer.nextflipcompany@gmail.com";
        public ViewSupporterDashboard(ILogger<ViewSupporterDashboard> logger)
        {
            _logger = logger;
        }

        //Forward Support Ticket Action
        [Route("ForwardSupportTicket")]
        [HttpPost]
        public async Task<IActionResult> ForwardSupportTicket([FromServices] ISendMailService sendMailService, [FromServices] ISupportTicketDAO supportTicketDAO, [FromForm] string btnAction, [FromForm] string supportTicketID, [FromForm] string userEmail, [FromForm] string topicName, [FromForm] string content)
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
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/Respond: " + ex.Message);
                return new JsonResult("An error occurred");
            }
        }

        //Get Support Ticket Details
        [Route("GetSupportTicketDetails/{supportTicketID}")]
        public IActionResult GetSupportTicketDetails([FromServices] ISupportTicketService supportTicketService, string supportTicketID)
        {
            try
            {
                SupportTicket supportTicket = supportTicketService.ViewSupportTicketByID(supportTicketID);
                return new JsonResult(supportTicket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetSupportTicketDetails: " + ex.Message);
                return new JsonResult(ex.Message);
            }
        }

        //Get All Support Topics
        [Route("GetAllSupportTopics")]
        public IActionResult GetAllSupportTopics([FromServices] ISupportTopicService supportTopicService)
        {
            try
            {
                IList<SupportTopic> supportTopics = supportTopicService.GetAllTopics();
                return new JsonResult(supportTopics);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetAllSupportTopics: " + ex.Message);
                return new JsonResult(ex.Message);
            }
        }

        [Route("ViewSupportTicketByTopic")]
        [HttpPost]
        public IActionResult ViewSupportTicketByTopic([FromServices] ISupportTicketService supportTicketService, [FromBody]Request request)
        {
            try
            {
                string topicName = request.TopicName;
                if ( topicName == null || topicName.Length == 0) throw new Exception("Invalid topic name");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage-1);

                IList<SupportTicket> supportTickets = supportTicketService.ViewSupportTicketByTopic(limit, offset, topicName);
                int totalPage = supportTicketService.GetNumOfSupportTicketsByTopic(topicName);

                return new JsonResult(new {TotalPage = totalPage, Data = supportTickets });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/ViewSupportTicketByTopic: " + ex.Message);
                return new JsonResult(ex.Message);
            }
        }

        [Route("SearchSupportTicketByTopic")]
        [HttpPost]
        public IActionResult SearchSupportTicketByTopic([FromServices] ISupportTicketService supportTicketService, [FromBody] Request request)
        {
            try
            {
                string topicName = request.TopicName;
                if (topicName == null || topicName.Length == 0) throw new Exception("Invalid topic name");
                string searchValue = request.SearchValue;
                if (searchValue == null || searchValue.Length == 0) throw new Exception("Invalid search value");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage - 1);
                IList<SupportTicket> supportTickets = supportTicketService.SearchSupportTicketByTopic(searchValue, topicName, limit, offset);
                int totalPage = supportTicketService.GetNumOfSupportTicketsByTopicAndSearch(searchValue, topicName);

                return new JsonResult(new { TotalPage = totalPage, Data = supportTickets });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/SearchSupportTicketByTopic: " + ex.Message);
                return new JsonResult(ex.Message);
            }
        }

        [Route("SearchSupportTicketByTopicAndStatus")]
        [HttpPost]
        public IActionResult SearchSupportTicketByTopicAndStatus([FromServices] ISupportTicketService supportTicketService, [FromBody] Request request)
        {
            try
            {
                string topicName = request.TopicName;
                if (topicName == null || topicName.Length == 0) throw new Exception("Invalid topic name");
                string searchValue = request.SearchValue;
                if (searchValue == null || searchValue.Length == 0) throw new Exception("Invalid search value");
                string status = request.Status;
                if (status == null || status.Length == 0) throw new Exception("Invalid status");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage-1);

                IList<SupportTicket> supportTickets = supportTicketService.SearchSupportTicketByTopicAndByStatus(searchValue, topicName, status, limit, offset);
                int totalPage = supportTicketService.GetNumOfSupportTicketsByTopicAndSearchAndStatus(searchValue, topicName, status);

                return new JsonResult(new { TotalPage = totalPage, Data = supportTickets });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/SearchSupportTicketByTopicAndStatus: " + ex.Message);
                return new JsonResult(ex.Message);
            }
        }
        [Route("ViewSupportTicketByTopicAndStatus")]
        public IActionResult ViewSupportTicketByTopicAndStatus([FromServices] ISupportTicketService supportTicketService, [FromBody] Request request)
        {
            try
            {
                string topicName = request.TopicName;
                if (topicName == null || topicName.Length == 0) throw new Exception("Invalid topic name");
                string status = request.Status;
                if (status == null || status.Length == 0) throw new Exception("Invalid status");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage - 1);

                IEnumerable<SupportTicket> supportTickets = supportTicketService.ViewSupportTicketByTopicAndStatus(topicName, status, limit, offset);
                int totalPage = supportTicketService.GetNumOfSupportTicketsByTopicAndStatus(topicName, status);

                return new JsonResult(new {TotalPage = totalPage, Data = supportTickets });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/ViewSupportTicketByTopicAndStatus: " + ex.Message);
                return new JsonResult(ex.Message);
            }
        }
    }

    public class Request
    {
        public int RowsOnPage { get; set; }
        public int RequestPage { get; set; }
        public string TopicName { get; set; }
        public string SearchValue { get; set; }
        public string Status { get; set; }
    }


}
