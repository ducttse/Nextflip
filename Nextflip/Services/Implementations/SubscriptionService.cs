using Nextflip.Models.subscription;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionDAO _subscriptionDAO;
        public SubscriptionService(ISubscriptionDAO subscriptionDAO)
        {
            _subscriptionDAO = subscriptionDAO;
        }
        public Subscription GetSubsciptionByUserID(string userID) => _subscriptionDAO.GetSubsciptionByUserID(userID);

        public bool UpdateExpiredDate(Subscription subsciption) => _subscriptionDAO.UpdateExpiredDate(subsciption);
        public bool PurchaseSubscription(string userID, int interval) => _subscriptionDAO.PurchaseSubscription(userID, interval);
    }
}
