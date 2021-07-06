using System;
using System.Collections.Generic;
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
    }
}
