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


        public IEnumerable<Account> GetAccountListByEmailFilterRoleStatus(string searchValue, string roleName, string status, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account " +
                            "Where roleName = @roleName and status = @status and userEmail LIKE @userEmail " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@roleName", roleName);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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

        public int NumberOfAccountsBySearchingFilterRoleStatus(string searchValue, string roleName, string status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                            "From account " +
                            "Where roleName = @roleName and status = @status and userEmail LIKE @userEmail ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@roleName", roleName);
                    command.Parameters.AddWithValue("@status", status);
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

        public bool InactiveAccount(string userID, string note)
        {
            bool result = false;
            try
            {
                if (GetDetailOfAccount(userID).status.Equals("Inactive")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE account " +
                        "SET status= 'Inactive', note=@note " +
                        "WHERE userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@note", note);
                        command.Parameters.AddWithValue("@userID", userID);
                        int rowEffects = command.ExecuteNonQuery();
                        if (rowEffects > 0)
                        {
                            result = true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
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


        public IEnumerable<Account> GetAccountListByEmail(string searchValue, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account " +
                            "Where userEmail LIKE @userEmail " +
                            "Order by status ASC " +
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
        public int NumberOfAccountsBySearching(string searchValue)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                            "From account " +
                            "Where userEmail LIKE @userEmail ";
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
 
        public IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, string status, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage-1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account " +
                            "Where roleName = @roleName and status = @status " +
                            "LIMIT @offset, @limit";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@roleName", roleName);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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
        public int NumberOfAccountsByRoleAndStatus(string roleName, string status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                                "From account " +
                                "Where roleName = @roleName and status = @status";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@roleName", roleName);
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

        public IEnumerable<Account> GetAccountListByEmailFilterRole(string searchValue, string roleName, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, status " +
                            "From account " +
                            "Where roleName = @roleName and userEmail LIKE @userEmail " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@roleName", roleName);
                        command.Parameters.AddWithValue("@userEmail", $"%{searchValue}%");
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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

        public int NumberOfAccountsBySearchingFilterRole(string searchValue, string roleName)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                            "From account " +
                            "Where roleName = @roleName and userEmail LIKE @userEmail " +
                            "Order by status ASC";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@roleName", roleName);
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

        public IEnumerable<Account> GetAccountsListOnlyByRole(string roleName, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
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
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
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

        public int NumberOfAccountsByRole(string roleName)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                                "From account " +
                                "Where roleName = @roleName";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@roleName", roleName);
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


        public Account GetDetailOfAccount(string userID)
        {
            Account account = null;
            try { 
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status " +
                                "From account " +
                                "Where userID = @userID";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userID", userID);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            account = new Account
                            {
                                userID = reader.GetString(0),
                                userEmail = reader.GetString(1),
                                roleName = reader.GetString(2),
                                fullname = reader.GetString(3),
                                dateOfBirth = reader.GetDateTime(4),
                                status = reader.GetString(5)
                            };
                        }
                    }
                }
                connection.Close();
            }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }

        public Account GetDetailOfInactiveAccount(string userID)
        {
            Account account = null;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, note " +
                                    "From account " +
                                    "Where userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = new Account
                                {
                                    userID = reader.GetString(0),
                                    userEmail = reader.GetString(1),
                                    roleName = reader.GetString(2),
                                    fullname = reader.GetString(3),
                                    dateOfBirth = reader.GetDateTime(4),
                                    status = reader.GetString(5),
                                    note = reader.GetString(6)
                                };
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }

        public bool ActiveAccount(string userID)
        {
            bool result = false;
            try
            {
                if (GetDetailOfAccount(userID).status.Equals("Active")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE account " +
                        "SET status= 'Active', note=null " +
                        "WHERE userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        int rowEffects = command.ExecuteNonQuery();
                        if (rowEffects > 0)
                        {
                            result = true;
                        }
                    }
                    connection.Close();
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
