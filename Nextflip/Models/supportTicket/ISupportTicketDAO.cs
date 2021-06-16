using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nextflip.Models.supportTicket
{
    public interface ISupportTicketDAO
    {
        public bool SendSupportTicket(string userEmail, string topicID, string content);
        public IList<SupportTicket> ViewPendingSupportTickets(int limit, int offset, string topicName);
        public SupportTicket ViewSupportTicketByID(string supportTicketID);
        public Task<bool> ForwardSupportTicket(string supportTicketID, string forwardDepartment);
        public int GetNumOfSupportTickets();
        public IList<SupportTicket> SearchSupportTicket(string searchValue);
    }
}
