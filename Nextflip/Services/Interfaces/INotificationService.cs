using Nextflip.Models.notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAllNotifications();
        IEnumerable<Notification> GetNotificationsWithPaging(int pageNum);
        Notification GetDetailOfNotification(int notificationID);
        IEnumerable<Notification> GetAllAvailableNotifications();
        bool AddNotification(string title, string content);
        int CountNotification();
    }
}
