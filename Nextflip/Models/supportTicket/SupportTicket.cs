using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nextflip.Models.supportTicket
{
    public class SupportTicket
    {
        [Key] public string supportTicketID { get; set; }
        [MaxLength(50)] public string userEmail { get; set; }
        [ForeignKey("topicName")] public string topicName { get; set; }
        public string status { get; set; }
        [MaxLength(4000)] public string content { get; set; }

        public SupportTicket()
        {

        }

        public SupportTicket(string supportTicketID, string userEmail, string topicName, string status, string content)
        {
            this.supportTicketID = supportTicketID;
            this.userEmail = userEmail;
            this.topicName = topicName;
            this.status = status;
            this.content = content;
        }

    }
}
