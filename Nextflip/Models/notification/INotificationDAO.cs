using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.notification
{
    public interface INotificationDAO
    {
        IEnumerable<Notification> GetAllNotifications();
        Notification GetDetailOfNotification(int notificationID);
        Notification AddNotification();
    }
}
