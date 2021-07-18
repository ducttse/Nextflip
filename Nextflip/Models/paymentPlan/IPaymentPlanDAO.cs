using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.paymentPlan
{
    public interface IPaymentPlanDAO
    {
        IList<PaymentPlan> GetPaymentPlan();
    }
}
