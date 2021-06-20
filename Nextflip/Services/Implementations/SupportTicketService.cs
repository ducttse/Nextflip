using Nextflip.Models.supportTicket;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketDAO _supportTicketDAO;

        public SupportTicketService(ISupportTicketDAO supportTicketDAO)
        {
            _supportTicketDAO = supportTicketDAO;
        }

        public Task<bool> ForwardSupportTicket(string supportTicketID, string forwardDepartment) => _supportTicketDAO.ForwardSupportTicket(supportTicketID, forwardDepartment);

        public int GetNumOfSupportTicketsByTopic(string topicName) => _supportTicketDAO.GetNumOfSupportTicketsByTopic(topicName);

        public int GetNumOfSupportTicketsByTopicAndSearch(string searchValue, string topicName) => _supportTicketDAO.GetNumOfSupportTicketsByTopicAndSearch(searchValue, topicName);

        public int GetNumOfSupportTicketsByTopicAndSearchAndStatus(string searchValue, string topicName, string status) => _supportTicketDAO.GetNumOfSupportTicketsByTopicAndSearchAndStatus(searchValue, topicName, status);

        public int GetNumOfSupportTicketsByTopicAndStatus(string topicName, string status) => _supportTicketDAO.GetNumOfSupportTicketsByTopicAndStatus(topicName, status);

        public IList<SupportTicket> SearchSupportTicketByTopic(string searchValue, string topicName, int limit, int offset) => _supportTicketDAO.SearchSupportTicketByTopic(searchValue, topicName, limit, offset);

        public IList<SupportTicket> SearchSupportTicketByTopicAndByStatus(string searchValue, string topicName, string status, int limit, int offset) => _supportTicketDAO.SearchSupportTicketByTopicAndByStatus(searchValue, topicName, status, limit, offset);

        public bool SendSupportTicket(string userEmail, string topicName, string content) => _supportTicketDAO.SendSupportTicket(userEmail, topicName, content);

        public SupportTicket ViewSupportTicketByID(string supportTicketID) => _supportTicketDAO.ViewSupportTicketByID(supportTicketID);

        public IList<SupportTicket> ViewSupportTicketByTopic(int limit, int offset, string topicName) => _supportTicketDAO.ViewSupportTicketByTopic(limit, offset, topicName);

        public IList<SupportTicket> ViewSupportTicketByTopicAndStatus(string topicName, string status, int limit, int offset) => _supportTicketDAO.ViewSupportTicketByTopicAndStatus(topicName, status, limit, offset);
    }
}
