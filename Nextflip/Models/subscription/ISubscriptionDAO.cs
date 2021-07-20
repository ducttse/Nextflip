﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.subscription
{
    public interface ISubscriptionDAO
    {
        bool UpdateExpiredDate(Subscription subsciption);

        Subscription GetSubsciptionByUserID(string userID);
        public bool PurchaseSubscription(string userID, int interval);
        bool PurchaseSubscription(string extensionFormUserId, DateTime extensionFormIssueTime, int extensionFormExtensionDays, int extensionFormPaymentPlanId);

        IEnumerable<object> GetSubscriptions(int rows, int page, string status);
        IEnumerable<object> GetSubscriptionsByUserEmail(string userEmail, int rows, int page,string status);
        bool RefundSubscription(string subscriptionID);

    }
}
