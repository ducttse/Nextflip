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
        public async Task<IActionResult> ForwardSupportTicket([FromServices] ISendMailService sendMailService, [FromServices] ISupportTicketDAO supportTicketDAO, [FromBody]ForwardDetails forwardDetails)
        {
            try
            {
                string body = "From " + forwardDetails.UserEmail + "------" + "SupportTicketID: " + forwardDetails.SupportTicketID + "\n" + forwardDetails.Content;
                string toEmail = null;

                if (forwardDetails.BtnAction.Equals("technical")) toEmail = TECHNICAL_EMAIL;
                else if (forwardDetails.BtnAction.Equals("customerRelation")) toEmail = CUSTOMER_RELATION_EMAIL;
                else throw new Exception("Email error occurred");
                await sendMailService.SendEmailAsync(toEmail, forwardDetails.TopicName, body);

                bool result = supportTicketDAO.ForwardSupportTicket(forwardDetails.SupportTicketID, toEmail).Result;
                if (result) return new JsonResult(new { Message = "Forward successful"});
                return new JsonResult(new {Message = "An Error Occurred" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/Respond: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }

        //Get Support Ticket Details
        [Route("GetSupportTicketDetails/{supportTicketID}")]
        public IActionResult GetSupportTicketDetails([FromServices] ISupportTicketService supportTicketService, string supportTicketID)
        {
            try
            {
                SupportTicket supportTicket = supportTicketService.ViewSupportTicketByID(supportTicketID);
                if (supportTicket == null) throw new Exception("No Support Ticket found");
                return new JsonResult(supportTicket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetSupportTicketDetails: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }

        //Get All Support Topics
        [Route("GetAllSupportTopics")]
        public IActionResult GetAllSupportTopics([FromServices] ISupportTopicService supportTopicService)
        {
            try
            {
                IList<SupportTopic> supportTopics = supportTopicService.GetAllTopics();
                if (supportTopics.Count == 0) throw new Exception("No Support Topic found");
                return new JsonResult(supportTopics);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/GetAllSupportTopics: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }

        [Route("ViewSupportTicketByTopic")]
        [HttpPost]
        public IActionResult ViewSupportTicketByTopic([FromServices] ISupportTicketService supportTicketService, [FromBody]SupporterRequest request)
        {
            try
            {
                string topicName = request.TopicName;
                if ( topicName == null || topicName.Trim().Length == 0) throw new Exception("Invalid topic name");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage-1);
                string sortBy = request.SortBy;
                if (sortBy == null || sortBy.Trim().Length == 0) sortBy = "createdDate";
                string according = request.According;
                if (according == null || according.Trim().Length == 0)
                {
                    according = "DESC";
                    if (!sortBy.Equals("createdDate")) according = "ASC";
                }

                IList<SupportTicket> supportTickets = supportTicketService.ViewSupportTicketByTopic(limit, offset, topicName, sortBy, according);
                int totalRecord = supportTicketService.GetNumOfSupportTicketsByTopic(topicName);
                int totalPage = totalRecord / limit + (totalRecord % limit == 0 ? 0 : 1);

                return new JsonResult(new {TotalPage = totalPage, Data = supportTickets });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/ViewSupportTicketByTopic: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }

        [Route("SearchSupportTicketByTopic")]
        [HttpPost]
        public IActionResult SearchSupportTicketByTopic([FromServices] ISupportTicketService supportTicketService, [FromBody] SupporterRequest request)
        {
            try
            {
                string topicName = request.TopicName;
                if (topicName == null || topicName.Trim().Length == 0) throw new Exception("Invalid topic name");
                string searchValue = request.SearchValue;
                if (searchValue == null || searchValue.Trim().Length == 0) throw new Exception("Invalid search value");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage - 1);
                string sortBy = request.SortBy;
                if (sortBy == null || sortBy.Trim().Length == 0) sortBy = "createdDate";
                string according = request.According;
                if (according == null || according.Trim().Length == 0)
                {
                    according = "DESC";
                    if (!sortBy.Equals("createdDate")) according = "ASC";
                }

                IList<SupportTicket> supportTickets = supportTicketService.SearchSupportTicketByTopic(searchValue, topicName, limit, offset, sortBy, according);
                int totalRecord = supportTicketService.GetNumOfSupportTicketsByTopicAndSearch(searchValue, topicName);
                int totalPage = totalRecord / limit + (totalRecord % limit == 0 ? 0 : 1);

                return new JsonResult(new { TotalPage = totalPage, Data = supportTickets });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/SearchSupportTicketByTopic: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }

        [Route("SearchSupportTicketByTopicAndStatus")]
        [HttpPost]
        public IActionResult SearchSupportTicketByTopicAndStatus([FromServices] ISupportTicketService supportTicketService, [FromBody] SupporterRequest request)
        {
            try
            {
                string topicName = request.TopicName;
                if (topicName == null || topicName.Trim().Length == 0) throw new Exception("Invalid topic name");
                string searchValue = request.SearchValue;
                if (searchValue == null || searchValue.Trim().Length == 0) throw new Exception("Invalid search value");
                string status = request.Status;
                if (status == null || status.Trim().Length == 0) throw new Exception("Invalid status");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage-1);
                string sortBy = request.SortBy;
                if (sortBy == null || sortBy.Trim().Length == 0) sortBy = "createdDate";
                string according = request.According;
                if (according == null || according.Trim().Length == 0)
                {
                    according = "DESC";
                    if (!sortBy.Equals("createdDate")) according = "ASC";
                }

                IList<SupportTicket> supportTickets = supportTicketService.SearchSupportTicketByTopicAndByStatus(searchValue, topicName, status, limit, offset, sortBy, according);
                int totalRecord = supportTicketService.GetNumOfSupportTicketsByTopicAndSearchAndStatus(searchValue, topicName, status);
                int totalPage = totalRecord / limit + (totalRecord % limit == 0 ? 0 : 1);

                return new JsonResult(new { TotalPage = totalPage, Data = supportTickets });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/SearchSupportTicketByTopicAndStatus: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }
        [Route("ViewSupportTicketByTopicAndStatus")]
        public IActionResult ViewSupportTicketByTopicAndStatus([FromServices] ISupportTicketService supportTicketService, [FromBody] SupporterRequest request)
        {
            try
            {
                string topicName = request.TopicName;
                if (topicName == null || topicName.Trim().Length == 0) throw new Exception("Invalid topic name");
                string status = request.Status;
                if (status == null || status.Trim().Length == 0) throw new Exception("Invalid status");
                int limit = request.RowsOnPage;
                int offset = request.RowsOnPage * (request.RequestPage - 1);
                string sortBy = request.SortBy;
                if (sortBy == null || sortBy.Trim().Length == 0) sortBy = "createdDate";
                string according = request.According;
                if (according == null || according.Trim().Length == 0)
                {
                    according = "DESC";
                    if (!sortBy.Equals("createdDate")) according = "ASC";
                }

                IList<SupportTicket> supportTickets = supportTicketService.ViewSupportTicketByTopicAndStatus(topicName, status, limit, offset, sortBy, according);
                int totalRecord = supportTicketService.GetNumOfSupportTicketsByTopicAndStatus(topicName, status);
                int totalPage = totalRecord / limit + (totalRecord % limit == 0 ? 0 : 1);

                return new JsonResult(new {TotalPage = totalPage, Data = supportTickets });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ViewSupporterDashboard/ViewSupportTicketByTopicAndStatus: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }
    }

    public partial class SupporterRequest

    {
        public int RowsOnPage { get; set; }
        public int RequestPage { get; set; }
        public string TopicName { get; set; }
        public string SearchValue { get; set; }
        public string Status { get; set; }
        public string SortBy { get; set; }
        public string According { get; set; }
    }

    public partial class ForwardDetails
    {
        public string BtnAction { get; set; }
        public string SupportTicketID { get; set; }
        public string UserEmail { get; set; }
        public string TopicName { get; set; }
        public string Content { get; set; }

    }

}
