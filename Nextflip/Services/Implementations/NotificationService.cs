using Nextflip.Models.notification;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationDAO _notificationDao;
        public NotificationService(INotificationDAO notificationDao) => _notificationDao = notificationDao;
        public IEnumerable<Notification> ViewAllNotifications(string status, int RowsOnPage, int RequestPage) => _notificationDao.ViewAllNotifications(status, RowsOnPage, RequestPage);
        public IEnumerable<Notification> ViewAvailableNotifications(int RowsOnPage, int RequestPage) => _notificationDao.ViewAvailableNotifications(RowsOnPage, RequestPage);
        public Notification GetDetailOfNotification(int notificationID) => _notificationDao.GetDetailOfNotification(notificationID);
        public IEnumerable<Notification> GetAllAvailableNotifications() => _notificationDao.GetAllAvailableNotifications();
        public bool AddNotification(string title, string content) => _notificationDao.AddNotification(title, content);
        public bool EditNotification(int notificationID, string title, string content, string status) => _notificationDao.EditNotification(notificationID,title,content,status);
        public int CountAvailableNotification() => _notificationDao.CountAvailableNotification();
        public int CountNotification(string status) => _notificationDao.CountNotification(status);
    }
}
