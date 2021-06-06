using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.account
{
    public class AccountDAO : BaseDAL, IAccountDAO
    {
        public AccountDAO() {}
        public static string ConnectionString { get; set; }

        public IEnumerable<Account> GetAccountListByEmail(string searchValue)
        {
            var accounts = new List<Account>();
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleID, fullname" +
                            "From account " +
                            "Where userEmail LIKE @userEmail";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "@userEmail";
                        param.Value = searchValue;
                        command.Parameters.Add(param);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read()) { 
                            accounts.Add(new Account
                            {
                                userID = reader.GetString(0),
                                userEmail = reader.GetString(1),
                                roleID = reader.GetInt32(2),
                                fullname = reader.GetString(3)
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
            finally
            {
                connection.Close();
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
