using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.account
{
    public interface IAccountDAO
    {
        IEnumerable<Account> GetAllAccounts();
        IEnumerable<Account> GetAccountListByEmail(string searchValue);
        bool ChangeAccountStatus(string userID);
        bool AddNewStaff(string fullname, string userEmail, string password, int intRole);
        bool EditStaffProfile(String userID, String fullname, DateTime dateOfBirth, int intRole);
        bool ChangeStaffPassword(String userID, String password);
        int NumberOfAccounts();
        IEnumerable<Account> GetAccountsListAccordingRequest(int NumberOfPage, int RowsOnPage, int RequestPage);
//        IEnumerable<Account> GetAllActiveAccounts();
//       IEnumerable<Account> GetAllInactiveAccounts();
    }

}
