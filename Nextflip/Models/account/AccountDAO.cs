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
                            "Where userEmail = @userEmail";
                    Debug.WriteLine(Sql);
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", searchValue);
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

        
    }
}
