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
        public IEnumerable<Notification> GetAllNotifications() => _notificationDao.GetAllNotifications();
        public IEnumerable<Notification> GetNotificationsWithPaging(int pageNum) => _notificationDao.GetNotificationsWithPaging(pageNum);
        public Notification GetDetailOfNotification(int notificationID) => _notificationDao.GetDetailOfNotification(notificationID);
        public IEnumerable<Notification> GetAllAvailableNotifications() => _notificationDao.GetAllAvailableNotifications();
        public bool AddNotification(string title, string content) => _notificationDao.AddNotification(title, content);
        public int CountNotification() => _notificationDao.CountNotification();
    }
}
