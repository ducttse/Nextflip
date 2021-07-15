using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.paymentPlan;

namespace Nextflip.Services.Interfaces
{
    public interface IPaymentPlanService
    {
        IList<PaymentPlan> GetPaymentPlan();
    }
}
