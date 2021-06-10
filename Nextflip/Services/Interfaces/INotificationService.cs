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
        Notification GetDetailOfNotification(int notificationID);
        IEnumerable<Notification> GetAllAvailableNotifications();
    }
}
