using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.paymentPlan;
using Nextflip.Services.Interfaces;

namespace Nextflip.Services.Implementations
{
    public class PaymentPlanService : IPaymentPlanService
    {
        private IPaymentPlanDAO _paymentPlanDao;

        public PaymentPlanService(IPaymentPlanDAO paymentPlanDao)
        {
            _paymentPlanDao = paymentPlanDao;
        }
        public IList<PaymentPlan> GetPaymentPlan()
        {
            return _paymentPlanDao.GetPaymentPlan();
        }
    }
}
