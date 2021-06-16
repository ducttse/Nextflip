using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.utils;

namespace Nextflip.Models.account
{
    public class AccountDAO : IAccountDAO
    {
        public AccountDAO() { }
        public IEnumerable<Account> GetAllAccounts()
        {
            var accounts = new List<Account>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Account
                                {
                                    userID = reader.GetString(0),
                                    userEmail = reader.GetString(1),
                                    roleName = reader.GetString(2),
                                    fullname = reader.GetString(3),
                                    status = reader.GetString(4)
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
            return accounts;
        }

        public IEnumerable<Account> GetAccountListByEmail(string searchValue)
        {
            var accounts = new List<Account>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account " +
                            "Where userEmail LIKE @userEmail";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read()) { 
                            accounts.Add(new Account
                            {
                                userID = reader.GetString(0),
                                userEmail = reader.GetString(1),
                                roleName = reader.GetString(2),
                                fullname = reader.GetString(3),
                                status = reader.GetString(4)
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
            return accounts;
        }
        
        public bool ChangeAccountStatus(string userID)
        {
            throw new NotImplementedException();
        }

        public bool AddNewStaff(string fullname, string userEmail, string password, int intRole)
        {
            throw new NotImplementedException();
        }

        public bool EditStaffProfile(string userID, string fullname, DateTime dateOfBirth, int intRole)
        {
            throw new NotImplementedException();
        }

        public bool ChangeStaffPassword(string userID, string password)
        {
            throw new NotImplementedException();
        }

        public int NumberOfAccounts()
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                                "From account";
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

        public IEnumerable<Account> GetAccountsListAccordingRequest(int NumberOfPage, int RowsOnPage, int RequestPage)
        {                                                                           
            var accounts = new List<Account>();
            int limit = NumberOfPage * RowsOnPage;
            int offset = ((int)(RequestPage / NumberOfPage)) * limit;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account " +
                            "LIMIT @offset, @limit";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", limit);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Account
                                {
                                    userID = reader.GetString(0),
                                    userEmail = reader.GetString(1),
                                    roleName = reader.GetString(2),
                                    fullname = reader.GetString(3),
                                    status = reader.GetString(4)
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
            return accounts;
        }
        /*
                public IEnumerable<Account> GetAllActiveAccounts()
                {
                    var accounts = new List<Account>();
                    try
                    {
                        using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                        {
                            connection.Open();
                            string Sql = "Select userID, userEmail, roleName, fullname, status " +
                                    "From account " +
                                    "Where status = 'Active' ";
                            Debug.WriteLine(Sql);
                            using (var command = new MySqlCommand(Sql, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        accounts.Add(new Account
                                        {
                                            userID = reader.GetString(0),
                                            userEmail = reader.GetString(1),
                                            roleName = reader.GetString(2),
                                            fullname = reader.GetString(3),
                                            status = reader.GetString(4)
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
                    return accounts;
                }

                public IEnumerable<Account> GetAllInactiveAccounts()
                {
                    var accounts = new List<Account>();
                    try
                    {
                        using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                        {
                            connection.Open();
                            string Sql = "Select userID, userEmail, roleName, fullname, status " +
                                    "From account " +
                                    "Where status = 'InActive' ";
                            Debug.WriteLine(Sql);
                            using (var command = new MySqlCommand(Sql, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        accounts.Add(new Account
                                        {
                                            userID = reader.GetString(0),
                                            userEmail = reader.GetString(1),
                                            roleName = reader.GetString(2),
                                            fullname = reader.GetString(3),
                                            status = reader.GetString(4)
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
                    return accounts;
                }
        */
        public IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, int NumberOfPage, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int limit = NumberOfPage * RowsOnPage;
            int offset = ((int)((RequestPage-1) / NumberOfPage)) * limit;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account " +
                            "Where roleName = @roleName " +
                            "LIMIT @offset, @limit";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@roleName", roleName);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", limit);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Account
                                {
                                    userID = reader.GetString(0),
                                    userEmail = reader.GetString(1),
                                    roleName = reader.GetString(2),
                                    fullname = reader.GetString(3),
                                    status = reader.GetString(4)
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
            return accounts;
        }
    }
}
