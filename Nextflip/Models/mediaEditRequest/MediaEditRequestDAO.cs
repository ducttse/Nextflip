﻿using MySql.Data.MySqlClient;
using Nextflip.Models.media;
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

        public IEnumerable<MediaEditRequest> GetPendingMediaByUserEmail(string searchValue, int RowsOnPage, int RequestPage)
        {
            var requests = new List<MediaEditRequest>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select requestID, userEmail, mediaID, status, note " +
                            "From mediaEditRequest " +
                            "Where userEmail LIKE @userEmail " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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

        public int NumberOfPendingMediasBySearching(string searchValue)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(requestID) " +
                            "From mediaEditRequest " +
                            "Where userEmail LIKE @userEmail";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
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

        public bool ChangeRequestStatus(int requestID, string status)
        {
            bool result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    //Editor_Request() = true;
                    string Sql = "Update mediaEditRequest " +
                            "Set status = @status " +
                            "Where requestID = @requestID";
                    MySqlCommand command = new MySqlCommand(Sql, connection);
                    command.Parameters.AddWithValue("@status", status);
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
        
        public bool ApproveRequest(int requestID)
        {
            bool result = false;
            try
            {
                if (!GetMediaEditRequestByID(requestID).status.Equals("Pending")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Update mediaEditRequest " +
                            "Set status = 'Approved' " +
                            "Where requestID = @requestID";
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

        public bool DisapproveRequest(int requestID, string note)
        {
            bool result = false;
            try
            {
                if (!GetMediaEditRequestByID(requestID).status.Equals("Pending")) return false;
                if (note == null || note.Trim().Equals("")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Update mediaEditRequest " +
                            "Set status = 'Disapproved', note = @note " +
                            "Where requestID = @requestID";
                    MySqlCommand command = new MySqlCommand(Sql, connection);
                    command.Parameters.AddWithValue("@requestID", requestID);
                    command.Parameters.AddWithValue("@note", note);
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

        public int NumberOfPendingMedias()
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(requestID) " +
                                "From mediaEditRequest";
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

        public IEnumerable<MediaEditRequest> GetPendingMediasListAccordingRequest(int RowsOnPage, int RequestPage)
        {
            var requests = new List<MediaEditRequest>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select requestID, userEmail, mediaID, status, note " +
                            "From mediaEditRequest " +
                            "LIMIT @offset, @limit";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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


        public int NumberOfPendingMediasFilterStatus(string status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(requestID) " +
                                "From mediaEditRequest " +
                                "Where status = @status";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
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

        public IEnumerable<MediaEditRequest> GetPendingMediasFilterStatus(string status, int RowsOnPage, int RequestPage)
        {
            var requests = new List<MediaEditRequest>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select requestID, userEmail, mediaID, status, note " +
                            "From mediaEditRequest " +
                            "Where status = @status " +
                            "LIMIT @offset, @limit";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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


        public IEnumerable<MediaEditRequest> GetPendingMediaByUserEmailFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage)
        {
            var requests = new List<MediaEditRequest>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select requestID, userEmail, mediaID, status, note " +
                            "From mediaEditRequest " +
                            "Where userEmail LIKE @userEmail and status = @Status " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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

        public bool AddMediaRequest(string userEmail, string mediaID, string note, string previewLink, string type, string ID)
        {
            bool result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Insert Into mediaEditRequest (userEmail, mediaID, status, note, previewLink, type, ID) " +
                            "Values (@userEmail, @mediaID, 'Pending', @note, @previewLink, @type, @ID) ";
                    MySqlCommand command = new MySqlCommand(Sql, connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    command.Parameters.AddWithValue("@mediaID", mediaID);
                    command.Parameters.AddWithValue("@note", note);
                    command.Parameters.AddWithValue("@previewLink", previewLink);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@ID", ID);
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

        public int NumberOfPendingMediasBySearchingFilterStatus(string searchValue, string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(requestID) " +
                            "From mediaEditRequest " +
                            "Where userEmail LIKE @userEmail and status = @Status";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                    command.Parameters.AddWithValue("@Status", Status);
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

        public MediaEditRequest GetMediaEditRequestByID(int requestID)
        {
            try
            {
                var request = new MediaEditRequest();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select requestID, userEmail, mediaID, status, note, previewLink " +
                                "From mediaEditRequest " +
                                "Where requestID = @requestID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@requestID", requestID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                request = new MediaEditRequest
                                {
                                    requestID = reader.GetInt32(0),
                                    userEmail = reader.GetString(1),
                                    mediaID = reader.GetString(2),
                                    status = reader.GetString(3),
                                    note = reader.GetString(4),
                                    previewLink = reader.GetString(5)
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<MediaEditRequest> GetMediaRequest(string status, string type, int RowsOnPage, int RequestPage)
        {
            var requests = new List<MediaEditRequest>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = null;
                    if (type.Equals("all") && status.Equals("all"))
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                              "From mediaEditRequest R, media M " +
                              "Where R.mediaID = M.mediaID " +
                            "LIMIT @offset, @limit";
                    } else if (type.Equals("all") && !status.Equals("all"))
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                              "From mediaEditRequest R, media M " +
                                "Where R.status = @status and R.mediaID = M.mediaID " +
                                "LIMIT @offset, @limit";
                    }
                    else if (!type.Equals("all") && status.Equals("all"))
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                              "From mediaEditRequest R, media M " +
                            "Where R.type = @type and R.mediaID = M.mediaID " +
                            "LIMIT @offset, @limit";
                    } else
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                              "From mediaEditRequest R, media M " +
                            "Where R.status = @status and R.type = @type and R.mediaID = M.mediaID " +
                            "LIMIT @offset, @limit";
                    }
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        if (!status.Equals("all")) command.Parameters.AddWithValue("@status", status);
                        if (!type.Equals("all")) command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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
                                    note = reader.GetString(4),
                                    previewLink = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    type = reader.GetString(6),
                                    ID = reader.GetString(7),
                                    mediaTitle = reader.GetString(8)
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

        public int NumberOfMediaRequest(string status, string type)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = null;
                if (type.Equals("all") && status.Equals("all"))
                {
                    Sql = "Select COUNT(requestID) " +
                           "From mediaEditRequest ";
                }
                else if (type.Equals("all") && !status.Equals("all"))
                {
                    Sql = "Select COUNT(requestID) " +
                        "From mediaEditRequest " +
                        "Where status = @status ";
                }
                else if (!type.Equals("all") && status.Equals("all"))
                {
                    Sql = "Select COUNT(requestID) " +
                        "From mediaEditRequest " +
                        "Where type = @type ";
                }
                else
                {
                    Sql = "Select COUNT(requestID) " +
                        "From mediaEditRequest " +
                        "Where status = @status and type = @type ";
                }
                using (var command = new MySqlCommand(Sql, connection))
                {
                    if (!status.Equals("all")) command.Parameters.AddWithValue("@status", status);
                    if (!type.Equals("all")) command.Parameters.AddWithValue("@type", type);
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
        public IEnumerable<MediaEditRequest> SearchingMediaRequest(string searchValue, string status, string type, int RowsOnPage, int RequestPage)
        {
            var requests = new List<MediaEditRequest>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = null;
                    if (type.Equals("all") && status.Equals("all"))
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                                "From mediaEditRequest R, media M " +
                               "Where R.userEmail LIKE @userEmail " +
                               "LIMIT @offset, @limit";
                    }
                    else if (type.Equals("all") && !status.Equals("all"))
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                            "From mediaEditRequest R, media M " +
                            "Where R.userEmail LIKE @userEmail and R.status = @status and R.MediaID = M.mediaID " +
                            "LIMIT @offset, @limit";
                    }
                    else if (!type.Equals("all") && status.Equals("all"))
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                            "From mediaEditRequest R, media M " +
                            "Where R.userEmail LIKE @userEmail and R.type = @type " +
                            "LIMIT @offset, @limit";
                    }
                    else
                    {
                        Sql = "Select R.requestID, R.userEmail, R.mediaID, R.status, R.note, R.previewLink, R.type, R.ID, M.title " +
                            "From mediaEditRequest R, media M " +
                            "Where R.userEmail LIKE @userEmail and R.status = @status and R.type = @type and R.mediaID = M.mediaID " +
                            "LIMIT @offset, @limit";
                    }
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                        if (!status.Equals("all")) command.Parameters.AddWithValue("@status", status);
                        if (!type.Equals("all")) command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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
                                    note = reader.GetString(4),
                                    previewLink = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    type = reader.GetString(6),
                                    ID = reader.GetString(7),
                                    mediaTitle = reader.GetString(8)
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

        public int NumberOfMediaRequestSearching(string searchValue, string status, string type)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = null;
                if (type.Equals("all") && status.Equals("all"))
                {
                    Sql = "Select COUNT(requestID) " +
                           "From mediaEditRequest " +
                           "Where userEmail LIKE @userEmail ";
                }
                else if (type.Equals("all") && !status.Equals("all"))
                {
                    Sql = "Select COUNT(requestID) " +
                        "From mediaEditRequest " +
                        "Where userEmail LIKE @userEmail and status = @status ";
                }
                else if (!type.Equals("all") && status.Equals("all"))
                {
                    Sql = "Select COUNT(requestID) " +
                        "From mediaEditRequest " +
                        "Where userEmail LIKE @userEmail and type = @type ";
                }
                else
                {
                    Sql = "Select COUNT(requestID) " +
                        "From mediaEditRequest " +
                        "Where userEmail LIKE @userEmail and status = @status and type = @type ";
                }
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                    if (!status.Equals("all")) command.Parameters.AddWithValue("@status", status);
                    if (!type.Equals("all")) command.Parameters.AddWithValue("@type", type);
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

