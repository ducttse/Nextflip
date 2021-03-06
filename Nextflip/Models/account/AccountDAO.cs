using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    status = reader.GetString("status")
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
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                            while (reader.Read())
                            {
                                accounts.Add(new Account
                                {
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")

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
                Account userAccount = GetDetailOfAccount(userID);
                if (userAccount.status.Equals("Inactive")) return false;
                if (note == null || note.Trim().Equals("")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    /*string Sql = "UPDATE account " +
                        "SET status= 'Inactive', note=@note " +
                        "WHERE userID = @userID";
                    */
                    string Sql = "disableAccount";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("userID", userID);
                        command.Parameters.AddWithValue("note", note);

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
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")
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
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")
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

        public IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, string status, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")
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
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")
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
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")
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
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")
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
                    string Sql = "Select userID, userEmail, roleName, fullname, dateOfBirth, status, pictureURL, note, token " +
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
                                    userID = reader.GetString("userID"),
                                    userEmail = reader.GetString("userEmail"),
                                    roleName = reader.GetString("roleName"),
                                    fullname = reader.GetString("fullname"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    status = reader.GetString("status"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL"),
                                    note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    token = reader.IsDBNull(reader.GetOrdinal("token")) ? null : reader.GetString("token")
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
                Account userAccount = GetDetailOfInactiveAccount(userID);
                if (userAccount.status.Equals("Active")) return false;
                if (userAccount.note == null || userAccount.note.Trim().Equals("")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE account " +
                        "SET status = 'Active', note='' " +
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

        public bool AddNewStaff(Account account)
        {
            try
            {
                bool isAdded = false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "insert into account(userID, hashedPassword, fullname, userEmail, roleName, dateOfBirth, status, pictureURL) " +
                                   "values(@userID, @hashedPassword, @fullname, @userEmail, @roleName, @dateOfBirth, @status, 'a')";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", generateID());
                        command.Parameters.AddWithValue("@hashedPassword", generateID());
                        string generateID()
                        {
                            return Guid.NewGuid().ToString("N");
                        }
                        command.Parameters.AddWithValue("@fullname", account.fullname);
                        command.Parameters.AddWithValue("@userEmail", account.userEmail);
                        command.Parameters.AddWithValue("@roleName", account.roleName.ToLower());
                        command.Parameters.AddWithValue("@dateOfBirth", account.dateOfBirth);
                        command.Parameters.AddWithValue("@status", "Active");
                        int rowAffect = command.ExecuteNonQuery();
                        if (rowAffect > 0) isAdded = true;
                    }
                    connection.Close();
                }
                return isAdded;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool UpdateStaffInfo(Account account)
        {
            try
            {
                bool isUpdate = false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Update account " +
                                "Set fullname = @fullname, roleName = @roleName, dateOfBirth = @dateOfBirth " +
                                "Where userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", account.userID);
                        command.Parameters.AddWithValue("@fullname", account.fullname);
                        command.Parameters.AddWithValue("@roleName", account.roleName.ToLower());
                        command.Parameters.AddWithValue("@dateOfBirth", account.dateOfBirth);
                        int rowAffect = command.ExecuteNonQuery();
                        if (rowAffect > 0) isUpdate = true;
                    }
                    connection.Close();
                }
                return isUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool IsSubscribedUser(string userID)
        {
            try
            {
                string roleName = "";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select roleName " +
                                "From account " +
                                "Where userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                roleName = reader.GetString(0);
                            }
                        }
                    }
                }
                return roleName.Equals("subscribed user", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Account GetAccountByID(string userID)
        {
            try
            {
                Account account = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select fullname, userEmail, dateOfBirth, roleName, pictureURL " +
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
                                    userID = userID,
                                    fullname = reader.GetString("fullname"),
                                    userEmail = reader.GetString("userEmail"),
                                    dateOfBirth = reader.GetDateTime("dateOfBirth"),
                                    roleName = reader.GetString("roleName"),
                                    pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL")
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool IsExistedEmail(string email)
        {
            try
            {
                bool isExisted = false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userEmail " +
                                "From account " +
                                "Where userEmail = @email ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        var reader = command.ExecuteReader();
                        if (reader.Read()) isExisted = true;
                    }
                    connection.Close();
                }
                return isExisted;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string RegisterAnAccount(string userEmail, string password, string fullname, DateTime dateOfBirth, string pictureURL, string token)
        {
            string roleName = "subscribed user";
            if (pictureURL == null) pictureURL = "https://storage.googleapis.com/next-flip/User%20Profile%20Image/Default";
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "registerAccount";
                    Debug.WriteLine(Sql);
                    Debug.WriteLine(fullname);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("userID_Output", MySqlDbType.String));
                        command.Parameters["userID_Output"].Direction = ParameterDirection.Output;

                        command.Parameters.AddWithValue("userEmail_Input", userEmail);
                        command.Parameters.AddWithValue("roleName_Input", roleName);
                        command.Parameters.AddWithValue("password_Input", password);
                        command.Parameters.AddWithValue("fullname_Input", fullname);
                        command.Parameters.AddWithValue("dateOfBirth_Input", dateOfBirth);
                        command.Parameters.AddWithValue("pictureURL_Input", pictureURL);
                        command.Parameters.AddWithValue("token_Input", token);

                        int result = command.ExecuteNonQuery();
                        connection.Close();
                        return (string)command.Parameters["userID_Output"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ChangeProfile(string userID, string fullname, DateTime dateOfBirth, string pictureURL)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE account " +
                                    "SET fullname = @fullname, dateOfBirth = @dateOfBirth, pictureURL = @pictureURL " +
                                    "WHERE (userID = @userID)";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        command.Parameters.AddWithValue("@fullname", fullname);
                        command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@pictureURL", pictureURL);
                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
        public Account Login(string email, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, hashedPassword,  fullname, dateOfBirth ,  status, pictureURL " +
                                "From account " +
                                "Where userEmail = @email AND hashedPassword = @password";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        var reader = command.ExecuteReader();
                        if (reader.Read()) return new Account
                        {
                            userID = reader.GetString("userID"),
                            userEmail = reader.GetString("userEmail"),
                            roleName = reader.GetString("roleName"),
                            fullname = reader.GetString("fullname"),
                            dateOfBirth = reader.GetDateTime("dateOfBirth"),
                            pictureURL = reader.GetString("pictureURL")

                        };
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }
        public bool ChangePassword(string userID, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE account " +
                                    "SET hashedPassword = @password " +
                                    "WHERE (userID = @userID);";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        command.Parameters.AddWithValue("@password", password);
                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        public Account CheckGoogleLogin(string googleEmail)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, hashedPassword,  fullname, dateOfBirth , status, pictureURL  " +
                                "From account " +
                                "Where userEmail = @googleEmail ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@googleEmail", googleEmail);
                        var reader = command.ExecuteReader();
                        if (reader.Read()) return new Account
                        {
                            userID = reader.GetString("userID"),
                            userEmail = reader.GetString("userEmail"),
                            roleName = reader.GetString("roleName"),
                            fullname = reader.GetString("fullname"),
                            dateOfBirth = reader.GetDateTime("dateOfBirth"),
                            pictureURL = reader.IsDBNull(reader.GetOrdinal("pictureURL")) ? null : reader.GetString("pictureURL")

                        };
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public Account ConfirmEmail(string userID, string token)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "confirmEmail";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("userID_Input", userID);
                        command.Parameters.AddWithValue("token_Input", token);
                        command.Parameters.Add(new MySqlParameter("role_Output", MySqlDbType.String));
                        command.Parameters["role_Output"].Direction = ParameterDirection.Output;
                        command.Parameters.Add(new MySqlParameter("fullname_Output", MySqlDbType.String));
                        command.Parameters["fullname_Output"].Direction = ParameterDirection.Output;
                        command.CommandType = CommandType.StoredProcedure;

                        command.ExecuteNonQuery();
                        connection.Close();
                        if (command.Parameters["role_Output"].Value == DBNull.Value) return null;
                        if (command.Parameters["fullname_Output"].Value == DBNull.Value) return null;
                        return new Account
                        {
                            userID = userID,
                            roleName = (string)command.Parameters["role_Output"].Value,
                            fullname = (string)command.Parameters["fullname_Output"].Value
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string ForgotPassword(string userEmail, string token)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "forgotPassword";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.Add(new MySqlParameter("userID_Output", MySqlDbType.String));
                        command.Parameters["userID_Output"].Direction = ParameterDirection.Output;
                        Debug.WriteLine("userID");
                        command.Parameters.AddWithValue("userEmail_Input", userEmail);
                        Debug.WriteLine(userEmail);
                        command.Parameters.AddWithValue("token_Input", token);
                        Debug.WriteLine(token);

                        command.CommandType = CommandType.StoredProcedure;
                        
                        command.ExecuteNonQuery();
                        connection.Close();
                        return (string)command.Parameters["userID_Output"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Account GetAccountByEmail(string email)
        {
            try
            {
                Account account = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, fullname, hashedPassword, roleName " +
                                    "From account " +
                                    "Where userEmail = @userEmail";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", email);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = new Account
                                {
                                    userID = reader.GetString("userID"),
                                    fullname = reader.GetString("fullname"),
                                    userEmail = email,
                                    hashedPassword = reader.GetString("hashedPassword"),
                                    roleName = reader.GetString("roleName")
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}