using Microsoft.AspNetCore.Mvc;
using Nextflip.Models.supportTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    public class ViewSupporterDashboard : ControllerBase
    {
        [Route("GetPendingSupportTickets")]
        public IActionResult GetPendingSupportTickets([FromServices] ISupportTicketDAO supportTicketDAO)
        {
            IList<SupportTicket> pendingSupportTickets = supportTicketDAO.ViewAllPendingSupportTickets();
            for(int i = 0; i < pendingSupportTickets.Count; i++)
            {
                Console.WriteLine(pendingSupportTickets.ElementAt(i).supportTicketID);
            }
            return new JsonResult(pendingSupportTickets);
        }

    }
}
