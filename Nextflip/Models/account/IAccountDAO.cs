using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.account
{
    public interface IAccountDAO
    {
        IEnumerable<Account> GetAccountListByEmail(string searchValue);
        Boolean ChangeAccountStatus(string userID);
        Boolean AddNewStaff(string fullname, string userEmail, string password, int intRole);
        Boolean EditStaffProfile(String userID, String fullname, DateTime dateOfBirth, int intRole);
        Boolean ChangeStaffPassword(String userID, String password);
    }
}
