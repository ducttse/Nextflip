using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nextflip.utils;

namespace Nextflip.Models.supportTicket
{
    public class SupportTicketDAO : ISupportTicketDAO
    {
        public SupportTicketDAO() { }

        public bool SendSupportTicket(string userEmail, string topicID, string content)
        {
            try {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {

                    connection.Open();
                    string sql = "INSERT INTO supportTicket(supportTicketID, userEmail, topicID, status, content) " +
                                                            "Value(@supportTicketID, @userEmail, @topicID, @status, @content);";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@supportTicketID", "randomString");  //use proc ???
                        command.Parameters.AddWithValue("@userEmail", userEmail);
                        command.Parameters.AddWithValue("@topicID", topicID);
                        command.Parameters.AddWithValue("@content", content);
                        using (var commit = command.ExecuteReaderAsync())
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

        public IList<SupportTicket> ViewPendingSupportTickets(int limit, int offset, string topicName)
        {
            IList<SupportTicket> supportTickets = new List<SupportTicket>();
            try {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString)) {
                    connection.Open();
                    string sql = "SELECT supportTicketID, userEmail, topicName, createdDate, status, content " +
                                    "FROM supportTicket " +
                                    "WHERE topicName = @topicName " +
                                    "Order By createdDate DESC " +
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
                                Console.WriteLine(createdDate);
                                string status = reader.GetString("status");
                                string content = reader.GetString("content");
                                supportTickets.Add(new SupportTicket(supportTicketID, userEmail, createdDate,topicName, status, content));
                            }
                            connection.Close();
                        }
                    }
                }
            return supportTickets;
            }
            catch(Exception e)
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
            MySqlTransaction transaction = null;
            try
            {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    await connection.OpenAsync();
                    string sql = "UPDATE supportTicket " +
                                    "SET status = 'Assigned', forwardDepartment = @forwardDepartment" +
                                    "WHERE supportTicketID = @supportTicketID; ";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@forwardDepartment", forwardDepartment);
                        command.Parameters.AddWithValue("@supportTicketID", supportTicketID);
                        await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                        await connection.CloseAsync();
                        return true;
                    }
                }
            }
            catch(Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message);
            }
        }

        public int GetNumOfSupportTickets()
        {
            try
            {
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select Count(*) From supportTicket;";
                    using (var command = new MySqlCommand(sql, connection))

                    {
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

        public IList<SupportTicket> SearchSupportTicket(string searchValue)
        {
            try
            {
                 IList<SupportTicket> supportTickets = new List<SupportTicket>();
                using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT supportTicketID, userEmail, topicName, createdDate, status, content " +
                                                            "FROM supportTicket " +
                                                            "WHERE userEmail like @searchValue OR MATCH (content)  AGAINST (@searchValue IN BOOLEAN MODE) " +
                                                            "Order By createdDate DESC; ";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string supportTicketID = reader.GetString("supportTicketID");
                                string userEmail = reader.GetString("userEmail");
                                string topicName = reader.GetString("topicName");
                                string createdDate = reader.GetMySqlDateTime("createdDate").ToString();
                                string status = reader.GetString("status");
                                string content = reader.GetString("content");
                                connection.Close();
                                supportTickets.Add( new SupportTicket(supportTicketID, userEmail, createdDate, topicName, status, content));
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
    }
}