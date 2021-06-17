﻿using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.notification
{
    public class NotificationDAO : INotificationDAO
    {
        public NotificationDAO() { }
        public IEnumerable<Notification> GetAllNotifications()
        {
            var notifications = new List<Notification>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select notificationID, title, status, publishedDate, content " +
                                "From notification " +
                                "Order By publishedDate DESC";
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
                                publishedDate = reader.GetDateTime(3),
                                content = reader.GetString(4)
                            });
                        }
                    }
                }
                connection.Close();
            }
            return notifications;
        }

       public IEnumerable<Notification> GetNotificationsListAccordingRequest(int RowsOnPage, int RequestPage)
        {
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            var notifications = new List<Notification>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select notificationID, title, status, publishedDate, content " +
                                "From notification " +
                                "Where status = 'Available' " +
                                "Order By publishedDate DESC " +
                                "Limit @offset, @limit";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@limit", RowsOnPage);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                notificationID = reader.GetInt32(0),
                                title = reader.GetString(1),
                                status = reader.GetString(2),
                                publishedDate = reader.GetDateTime(3),
                                content = reader.GetString(4)
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
                                content = reader.GetString(4)
                            };
                        }
                    }
                }
                connection.Close();
            }
            return notification;
        }

        public IEnumerable<Notification> GetAllAvailableNotifications()
        {
            var notifications = new List<Notification>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select notificationID, title, status, publishedDate, content " +
                                "From notification " +
                                "Where status = 'Available' " +
                                "Order By publishedDate DESC";
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
                                publishedDate = reader.GetDateTime(3),
                                content = reader.GetString(4)
                            });
                        }
                    }
                }
                connection.Close();
            }
            return notifications;
        }

        public bool AddNotification(string title, string content)
        {
            var result = false;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Insert Into notification (title,status,publishedDate,content) " +
                    "Values (@title,'Available',@publishedDate,@content)";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@publishedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@content", content);
                    int rowEffects = command.ExecuteNonQuery();
                    if (rowEffects > 0)
                    {
                        result = true;
                    }
                }
                connection.Close();
            }
            return result;
        }

        public int CountNotification()
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(notificationID) " +
                                "From notification";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }
    }
}
