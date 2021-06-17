using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.notification
{
    public interface INotificationDAO
    {
        IEnumerable<Notification> GetAllNotifications();
        IEnumerable<Notification> GetNotificationsListAccordingRequest(int RowsOnPage, int RequestPage);
        Notification GetDetailOfNotification(int notificationID);
        IEnumerable<Notification> GetAllAvailableNotifications();
        bool AddNotification(string title, string content);
        int CountNotification();
    }
}
