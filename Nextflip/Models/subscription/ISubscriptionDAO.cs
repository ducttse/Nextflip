using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.subscription
{
    public interface ISubscriptionDAO
    {
        bool UpdateExpiredDate(Subscription subsciption);

        Subscription GetSubsciptionByUserID(string userID);
    }
}
