using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nextflip.utils;
namespace Nextflip.Models.subscription
{
    public class SubscriptionDAO : ISubscriptionDAO
    {
        public bool UpdateExpiredDate(Subscription subsciption)
        {
            try
            {
                bool isUpdated = false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Update subscription " +
                                "Set status = @status, startDate = @startDate,endDate = @endDate " +
                                "Where userID = @userID;";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@status", "Active");
                        command.Parameters.AddWithValue("@startDate", subsciption.StartDate);
                        command.Parameters.AddWithValue("@endDate", subsciption.EndDate);
                        command.Parameters.AddWithValue("@userID", subsciption.UserID);
                        int rowAffect = command.ExecuteNonQuery();
                        if(rowAffect >0)
                        {
                            isUpdated = true;
                        }
                    }
                }
                return isUpdated;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public Subscription GetSubsciptionByUserID(string userID)
        {
            try
            {
                Subscription subsciption = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select SubscriptionID, Status, StartDate, EndDate " + 
                                "From subscription " +
                                "Where userID = @userID AND Status = 'Active' ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        using(var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                subsciption = new Subscription
                                {
                                    SubscriptionID = reader.GetString(0),
                                    UserID = userID,
                                    Status = reader.GetString(1),
                                    StartDate = reader.GetDateTime(2),
                                    EndDate = reader.GetDateTime(3)
                                };
                            }
                        }
                    }
                }
                return subsciption;
            }
            catch ( Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool PurchaseSubscription(string userID, int interval)
        {
            Random random = new Random();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO subscription(subscriptionID, userID, status ,startDate, endDate) "
                                + "VALUE(@subscriptionID, @userID, 'Active', @startDate, @endDate)";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@subscriptionID", random.Next(0, 100000));
                        command.Parameters.AddWithValue("@userID", userID);
                        command.Parameters.AddWithValue("@startDate", DateTime.Now);
                        command.Parameters.AddWithValue("@endDate", DateTime.Now.AddDays(interval));
                        int rowAffect = command.ExecuteNonQuery();
                        if (rowAffect > 0) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        public bool PurchaseSubscription(string userId, DateTime issueTime, int extensionDays,
            int paymentPlanId)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "ExtendSubscription";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("userId", userId);
                        command.Parameters.AddWithValue("issueDate", issueTime);
                        command.Parameters.AddWithValue("paymentPlanId", paymentPlanId);
                        command.Parameters.AddWithValue("duration", extensionDays);
                        int result = command.ExecuteNonQuery();
                        return (result > 0);
                    }
                }

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public DateTime GetExpiredDate(string userID)
        {
            DateTime result = DateTime.MinValue;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    Debug.WriteLine(userID);
                    string Sql = @"SELECT max(endDate) FROM subscription " +
                                 "WHERE userID = @userID AND status = 'approved'";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                            //    result = reader.GetDateTime("endDate");
                                if (!reader.IsDBNull(0))
                                {
                                    result = reader.GetDateTime(0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return result;
        }
    }
}
