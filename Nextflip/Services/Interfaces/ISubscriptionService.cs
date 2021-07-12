using Nextflip.Models.subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ISubscriptionService
    {
        bool UpdateExpiredDate(Subscription subsciption);

        Subscription GetSubsciptionByUserID(string userID);
        public bool PurchaseSubscription(string userID, int interval);
    }
}
