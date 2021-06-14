using Nextflip.Models.supportTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ISupportTicketService
    {
        public bool SendSupportTicket(string userEmail, string topicName, string content);
        public IList<SupportTicket> ViewPendingSupportTickets(int limit, int offset);
        public SupportTicket ViewSupportTicketByID(string supportTicketID);
        public int GetNumOfSupportTickets();
    }
}
