using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.notification
{
    public class NotificationDAO : INotificationDAO
    {
        public IEnumerable<Notification> GetAllNotifications()
        {
            var notifications = new List<Notification>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select notificationID, title, status, content " +
                                "From notification";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                notificationID = reader.GetInt32(0),
                                title = reader.GetString(1),
                                status = reader.GetString(2),
                                content = reader.GetString(3),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return notifications;
        }

        public Notification GetDetailOfNotification(int notificationID)
        {
            Notification notification = null;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select notificationID, title, status, publishedDate, content " +
                                "From notification " +
                                "Where notificationID = @notificationID";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@notificationID", notificationID);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            notification = new Notification
                            {
                                notificationID = reader.GetInt32(0),
                                title = reader.GetString(1),
                                status = reader.GetString(2),
                                publishedDate = reader.GetDateTime(3),
                                content = reader.GetString(4),
                            };
                        }
                    }
                }
                connection.Close();
            }
            return notification;
        }

        public Notification AddNotification()
        {
            throw new NotImplementedException();
        }

    }
}
