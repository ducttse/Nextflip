using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.subscription
{
    public class Subscription
    {
        public string SubscriptionID { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
    }
}
