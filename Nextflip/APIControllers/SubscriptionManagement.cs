using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.paymentPlan;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionManagement : ControllerBase
    {
        private readonly ILogger _logger;
        public SubscriptionManagement(ILogger<SubscriptionManagement> logger)
        {
            _logger = logger;
        }


        [Route("CheckWallet")]
        [HttpPost]
        public IActionResult CheckWallet([FromServices] IAccountService accountService, [FromBody] PaymentForm form)
        {
            try
            {
                bool result = accountService.CheckWallet(form.UserID, form.Money);
                if(result) return new JsonResult(new { Message = "Valid" });
                return new JsonResult(new { Message = "Insufficient money in your Wallet" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Subscription/CheckWallet: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("ConfirmPayment")]
        [HttpPost]
        public IActionResult ConfirmPayment([FromServices] IAccountService accountService, [FromServices]ISubscriptionService subscriptionService, [FromBody] PaymentForm form)
        {
            try
            {
                if (accountService.PurchaseSubscription(form.UserID, form.Money))
                {
                    bool result = subscriptionService.PurchaseSubscription(form.UserID, form.ExtensionDays);
                    if (result) return new JsonResult(new { Message = "Purchase Successfully ! Your subscription has been extended !" });
                }
                return new JsonResult(new { Message = "Payment Error ! Please try again !" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Subscription/CheckWallet: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("ExtendSubscription")]
        [HttpPost]
        public IActionResult ExtendSubscription([FromServices] ISubscriptionService subscriptionService, [FromBody] ExtensionForm extensionForm)
        {
            try
            {
                bool result = subscriptionService.PurchaseSubscription(extensionForm);
                if (result) return new JsonResult(new { Message = "Purchase Successfully ! Your subscription has been extended !" });
                return new JsonResult(new { Message = "Payment Error ! Please try again !" });
            }
            catch (Exception exception)
            {
                {
                    _logger.LogInformation("Subscription/ExtendSubscription: " + exception.Message);
                    return new JsonResult(new { Message = exception.Message });
                }
            }
        }

        public partial class PaymentForm
        {
            public string UserID { get; set; }
            public int ExtensionDays { get; set; }
            public double Money { get; set; }

        }
        public partial class ExtensionForm
        {
            public string UserId { get; set; }
            public int ExtensionDays { get; set; }
            public int PaymentPlanId { get; set; }

            public DateTime IssueTime { get; set; }
        }

        [Route("GetPaymentPlan")]
        [HttpGet]
        public IActionResult GetPaymentPlan([FromServices] IPaymentPlanService paymentPlanService)
        {
            try
            {
                IList<PaymentPlan> result = paymentPlanService.GetPaymentPlan();
                return new JsonResult(result);
            }
            catch (Exception exception)
            {
                {
                    _logger.LogInformation("Subscription/GetPaymentPlan: " + exception.Message);
                    return new JsonResult(null);
                }
            }
        }
    }
}
