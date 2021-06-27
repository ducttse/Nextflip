using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using MySql.Data.MySqlClient;
using Nextflip.utils;

namespace Nextflip.Models.supportTicket
{
    public class SupportTicketDAO : ISupportTicketDAO
    {
        public SupportTicketDAO() { }

        public bool SendSupportTicket(string userEmail, string topicName, string content)
        {
            try {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {

                    connection.Open();
                    string sql = "INSERT INTO supportTicket(supportTicketID, userEmail, topicName, status, content) " +
                                                            "Value(@supportTicketID, @userEmail, @topicName, 'Pending', @content);";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@supportTicketID", "randomString");  //use proc ???
                        command.Parameters.AddWithValue("@userEmail", userEmail);
                        command.Parameters.AddWithValue("@topicName", topicName);
                        command.Parameters.AddWithValue("@content", content);
                        using (var commit = command.ExecuteReader())
                        {
                            if (commit != null)
                            {
                                connection.Close();
                                return true;
                            }
                            connection.Close();
                        }

                    }
                }

                return false;
            }
            catch(Exception e)
        {
            throw new Exception(e.Message);
        }

        }

        public IList<SupportTicket> ViewSupportTicketByTopic(int limit, int offset, string topicName, string sortBy, string according)
        {
            string whereCondition = "";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName ";
            IList<SupportTicket> supportTickets = new List<SupportTicket>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT supportTicketID, userEmail, topicName, createdDate, status, content " +
                                    "FROM supportTicket " +
                                     whereCondition  +
                                    "Order By " + sortBy + " " + according + " "+
                                    "LIMIT @limit OFFSET @offset; ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@limit", limit);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@topicName", topicName);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string supportTicketID = reader.GetString("supportTicketID");
                                string userEmail = reader.GetString("userEmail");
                                string createdDate = reader.GetMySqlDateTime("createdDate").ToString();
                                string status = reader.GetString("status");
                                string content = reader.GetString("content");
                                supportTickets.Add(new SupportTicket(supportTicketID, userEmail, createdDate, topicName, status, content));
                            }
                            connection.Close();
                        }
                    }
                }
                return supportTickets;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int GetNumOfSupportTicketsByTopic(string topicName)
        {
            string whereCondition = "";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName ;";
            try
            {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select Count(*) " +
                                    "From supportTicket " +
                                    whereCondition;
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@topicName", topicName);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int numOfSupportTicket = reader.GetUInt16(0);
                                connection.Close();
                                return numOfSupportTicket;
                            }
                            connection.Close();
                        }
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IList<SupportTicket> ViewSupportTicketByTopicAndStatus(string topicName, string status, int limit, int offset, string sortBy, string according)
        {
            string whereCondition = "WHERE ";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName AND ";
            IList<SupportTicket> supportTickets = new List<SupportTicket>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT supportTicketID, userEmail, topicName, createdDate, status, content " +
                                    "FROM supportTicket " +
                                    whereCondition +"status = @status " +
                                    "Order By " + sortBy + " " + according + " "+
                                    "LIMIT @limit OFFSET @offset; ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@limit", limit);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@topicName", topicName);
                        command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string supportTicketID = reader.GetString("supportTicketID");
                                string userEmail = reader.GetString("userEmail");
                                string createdDate = reader.GetMySqlDateTime("createdDate").ToString();
                                string content = reader.GetString("content");
                                supportTickets.Add(new SupportTicket(supportTicketID, userEmail, createdDate, topicName, status, content));
                            }
                            connection.Close();
                        }
                    }
                }
                return supportTickets;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int GetNumOfSupportTicketsByTopicAndStatus(string topicName, string status)
        {
            string whereCondition = "";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName AND";
            try
            {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select Count(*) " +
                                    "From supportTicket " +
                                    whereCondition + "status = @status; ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@topicName", topicName);
                        command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int numOfSupportTicket = reader.GetUInt16(0);
                                connection.Close();
                                return numOfSupportTicket;
                            }
                            connection.Close();
                        }
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public SupportTicket ViewSupportTicketByID(string supportTicketID)
        {
            try
            {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT supportTicketID, userEmail, topicName, createdDate, status, content " +
                                                            "FROM supportTicket " +
                                                            "WHERE supportTicketID = @supportTicketID;";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@supportTicketID", supportTicketID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string userEmail = reader.GetString("userEmail");
                                string topicName = reader.GetString("topicName");
                                string createdDate = reader.GetMySqlDateTime("createdDate").ToString();
                                string status = reader.GetString("status");
                                string content = reader.GetString("content");
                                connection.Close();
                                return new SupportTicket(supportTicketID, userEmail, createdDate,topicName, status, content);
                            }
                            connection.Close();
                        }
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> ForwardSupportTicket(string supportTicketID, string forwardDepartment)
        {
            try
            {
                var connection = new MySqlConnection(DbUtil.ConnectionString);
                await connection.OpenAsync();
                string sql = "Update supportTicket " +
                                "SET forwardDepartment = @forwardDepartment, status = 'Assigned' " +
                                "WHERE supportTicketID = @supportTicket;";
                var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@forwardDepartment", forwardDepartment);
                command.Parameters.AddWithValue("@supportTicket", supportTicketID);
                int result = await command.ExecuteNonQueryAsync();
                if (result != 1) throw new Exception("SQL Error occurred");
                await connection.CloseAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }


        public IList<SupportTicket> SearchSupportTicketByTopic(string searchValue, string topicName, int limit, int offset, string sortBy, string according)

        {
            IList<SupportTicket> supportTickets = new List<SupportTicket>();
            string whereCondition = "WHERE ";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName AND";
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT supportTicketID, userEmail, topicName, createdDate, status, content " +
                                    "FROM supportTicket " +
                                    whereCondition + "(userEmail like @searchValue OR content like @searchValue) " +
                                    "Order By " + sortBy + " " + according + " "+
                                    "LIMIT @limit OFFSET @offset ; ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@limit", limit);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
                        command.Parameters.AddWithValue("@topicName", topicName);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string supportTicketID = reader.GetString("supportTicketID");
                                string userEmail = reader.GetString("userEmail");
                                string createdDate = reader.GetMySqlDateTime("createdDate").ToString();
                                string status = reader.GetString("status");
                                string content = reader.GetString("content");
                                supportTickets.Add(new SupportTicket(supportTicketID, userEmail, createdDate, topicName, status, content));
                            }
                            connection.Close();
                        }
                    }
                }
                return supportTickets;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int GetNumOfSupportTicketsByTopicAndSearch(string searchValue, string topicName)
        {
            string whereCondition = "WHERE ";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName AND ";
            try
            {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select Count(*) " +
                                    "From supportTicket " +
                                    whereCondition + "(userEmail like @searchValue OR content like @searchValue); ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
                        command.Parameters.AddWithValue("@topicName", topicName);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int numOfSupportTicket = reader.GetUInt16(0);
                                connection.Close();
                                return numOfSupportTicket;
                            }
                            connection.Close();
                        }
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IList<SupportTicket> SearchSupportTicketByTopicAndByStatus(string searchValue, string topicName, string status, int limit, int offset, string sortBy, string according)

        {
            string whereCondition = "WHERE ";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName AND";
            IList<SupportTicket> supportTickets = new List<SupportTicket>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT supportTicketID, userEmail, topicName, createdDate, status, content " +
                                    "FROM supportTicket " +
                                    whereCondition + "status = @status AND (userEmail like @searchValue OR content like @searchValue) " +
                                    "Order By " + sortBy + " " + according + " "+
                                    "LIMIT @limit OFFSET @offset; ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@limit", limit);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@topicName", topicName);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string supportTicketID = reader.GetString("supportTicketID");
                                string userEmail = reader.GetString("userEmail");
                                string createdDate = reader.GetMySqlDateTime("createdDate").ToString();
                                string content = reader.GetString("content");
                                supportTickets.Add(new SupportTicket(supportTicketID, userEmail, createdDate, topicName, status, content));
                            }
                            connection.Close();
                        }
                    }
                }
                return supportTickets;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int GetNumOfSupportTicketsByTopicAndSearchAndStatus(string searchValue, string topicName, string status)
        {
            string whereCondition = "WHERE ";
            if (!topicName.Equals("All")) whereCondition = "WHERE topicName = @topicName AND ";
            try
            {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select Count(*) " +
                                    "From supportTicket " +
                                    whereCondition + "status = @status AND (userEmail like @searchValue OR content like @searchValue) ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@topicName", topicName);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int numOfSupportTicket = reader.GetUInt16(0);
                                connection.Close();
                                return numOfSupportTicket;
                            }
                            connection.Close();
                        }
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}