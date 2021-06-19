using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.account
{
    public interface IAccountDAO
    {
        IEnumerable<Account> GetAllAccounts();
        IEnumerable<Account> GetAccountListByEmailFilterRoleStatus(string searchValue, string roleName, string status, int RowsOnPage, int RequestPage);
        int NumberOfAccountsBySearchingFilterRoleStatus(string searchValue, string roleName, string status);
        bool ChangeAccountStatus(string userID);
        bool AddNewStaff(string fullname, string userEmail, string password, int intRole);
        bool EditStaffProfile(String userID, String fullname, DateTime dateOfBirth, int intRole);
        bool ChangeStaffPassword(String userID, String password);
        int NumberOfAccountsBySearching(string searchValue);
        int NumberOfAccountsByRoleAndStatus(string roleName, string status);
        IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, string status, int RowsOnPage, int RequestPage);
        //        IEnumerable<Account> GetAllActiveAccounts();
        //       IEnumerable<Account> GetAllInactiveAccounts();
        IEnumerable<Account> GetAccountListByEmail(string searchValue, int RowsOnPage, int RequestPage);

        IEnumerable<Account> GetAccountListByEmailFilterRole(string searchValue, string roleName, int RowsOnPage, int RequestPage);
        int NumberOfAccountsBySearchingFilterRole(string searchValue, string roleName);

    }

}
