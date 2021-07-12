using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nextflip.Models.supportTicket
{
    public interface ISupportTicketDAO
    {
        public bool SendSupportTicket(string userEmail, string topicName, string content);
        public IList<SupportTicket> ViewSupportTicketByTopic(int limit, int offset, string topicName, string sortBy, string according);
        public int GetNumOfSupportTicketsByTopic(string topicName);
        public IList<SupportTicket> ViewSupportTicketByTopicAndStatus(string topicName, string status, int limit, int offset, string sortBy, string according);
        public int GetNumOfSupportTicketsByTopicAndStatus(string topicName, string status);
        public SupportTicket ViewSupportTicketByID(string supportTicketID);
        public Task<bool> ForwardSupportTicket(string supportTicketID, string forwardDepartment);
        public IList<SupportTicket> SearchSupportTicketByTopic(string searchValue, string topicName, int limit, int offset, string sortBy, string according);
        public int GetNumOfSupportTicketsByTopicAndSearch(string searchValue, string topicName);
        public IList<SupportTicket> SearchSupportTicketByTopicAndByStatus(string searchValue, string topicName, string status, int limit, int offset, string sortBy, string according);
        public int GetNumOfSupportTicketsByTopicAndSearchAndStatus(string searchValue, string topicName, string status);


    }
}
