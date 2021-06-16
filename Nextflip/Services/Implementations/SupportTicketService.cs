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

        public int GetNumOfSupportTickets() => _supportTicketDAO.GetNumOfSupportTickets();

        public bool SendSupportTicket(string userEmail, string topicName, string content) => _supportTicketDAO.SendSupportTicket(userEmail, topicName, content);

        public IList<SupportTicket> ViewPendingSupportTickets(int limit, int offset) => _supportTicketDAO.ViewPendingSupportTickets(limit, offset);

        public SupportTicket ViewSupportTicketByID(string supportTicketID) => _supportTicketDAO.ViewSupportTicketByID(supportTicketID);

    }
}
