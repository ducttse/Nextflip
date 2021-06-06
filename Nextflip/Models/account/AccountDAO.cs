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
        public IEnumerable<Account> GetAccountListByEmail(string searchValue)
        {
            var accounts = new List<Account>();
            IDataReader dataReader = null;
            string Sql = "Select userID, userEmail, roleID, fullname" +
                        "From account " +
                        "Where userEmail LIKE @userEmail";
            try
            {
                var param = dataProvider.CreateParameter("@userEmail", 40, $"%{searchValue}%", DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    accounts.Add(new Account
                    {
                        userID = dataReader.GetString(0),
                        userEmail = dataReader.GetString(1),
                        roleID = dataReader.GetInt32(2),
                        fullname = dataReader.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
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
