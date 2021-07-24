using Nextflip.Models.notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface INotificationService
    {
        IEnumerable<Notification> ViewAllNotifications(string status, int RowsOnPage, int RequestPage);
        IEnumerable<Notification> ViewAvailableNotifications(int RowsOnPage, int RequestPage);
        Notification GetDetailOfNotification(int notificationID);
        IEnumerable<Notification> GetAllAvailableNotifications();
        bool AddNotification(string title, string content);
        bool EditNotification(int notificationID, string title, string content, string status);
        int CountNotification(string status);
        int CountAvailableNotification();
        IEnumerable<Notification> SearchNotifications(string searchValue, string status, int RowsOnPage, int RequestPage);
        int CountSearchNotification(string searchValue, string status);
    }
}
