using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.paymentPlan
{
    public class PaymentPlan
    {
        public int PaymentPlanID { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
    }
}
