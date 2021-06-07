using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaEditRequest
{
    public class MediaEditRequestDAO : IMediaEditRequestDAO
    {
        public IEnumerable<MediaEditRequest> GetAllPendingMedias()
        {
            var requests = new List<MediaEditRequest>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select requestID, userEmail, mediaID, status, note " +
                            "From mediaEditRequest";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Add(new MediaEditRequest
                                {
                                    requestID = reader.GetInt32(0),
                                    userEmail = reader.GetString(1),
                                    mediaID = reader.GetString(2),
                                    status = reader.GetString(3),
                                    note = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return requests;
        }

        public IEnumerable<MediaEditRequest> GetPendingMediaByUserEmail(string searchValue)
        {
            var requests = new List<MediaEditRequest>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select requestID, userEmail, mediaID, status, note " +
                            "From mediaEditRequest " +
                            "Where userEmail = @userEmail";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", searchValue);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Add(new MediaEditRequest
                                {
                                    requestID = reader.GetInt32(0),
                                    userEmail = reader.GetString(1),
                                    mediaID = reader.GetString(2),
                                    status = reader.GetString(3),
                                    note = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return requests;
        }

        public bool ApproveRequest(int requestID)
        {
            bool result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    //Editor_Request() = true;
                    string Sql = "Update mediaEditRequest " +
                            "Set status = 'Approved' " +
                            "Where requestID = @requestID";
                    Debug.WriteLine(Sql);
                    MySqlCommand command = new MySqlCommand(Sql, connection);
                        command.Parameters.AddWithValue("@requestID", requestID);
                        int rows = command.ExecuteNonQuery();
                        if (rows > 0) result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool DisapproveRequest(int requestID)
        {
            bool result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    //Editor_Request() = false;
                    string Sql = "Update mediaEditRequest " +
                            "Set status = 'Disapproved' " +
                            "Where requestID = @requestID";
                    Debug.WriteLine(Sql);
                    MySqlCommand command = new MySqlCommand(Sql, connection);
                    command.Parameters.AddWithValue("@requestID", requestID);
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0) result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

    }
}
