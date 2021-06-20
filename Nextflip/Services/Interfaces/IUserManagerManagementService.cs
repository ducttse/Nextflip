using Nextflip.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IUserManagerManagementService
    {
        IEnumerable<Account> GetAllAccounts();
        IEnumerable<Account> GetAccountListByEmailFilterRoleStatus(string searchValue, string roleName, string status, int RowsOnPage, int RequestPage);
        int NumberOfAccountsBySearchingFilterRoleStatus(string searchValue, string roleName, string status);
        int NumberOfAccountsBySearching(string searchValue);
        int NumberOfAccountsByRoleAndStatus(string roleName, string status);
        IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, string status, int RowsOnPage, int RequestPage);
        //        IEnumerable<Account> GetAllActiveAccounts();
        //        IEnumerable<Account> GetAllInactiveAccounts();
        IEnumerable<Account> GetAccountListByEmail(string searchValue, int RowsOnPage, int RequestPage);
        IEnumerable<Account> GetAccountListByEmailFilterRole(string searchValue, string roleName, int RowsOnPage, int RequestPage);
        int NumberOfAccountsBySearchingFilterRole(string searchValue, string roleName);

        IEnumerable<Account> GetAccountsListOnlyByRole(string roleName, int RowsOnPage, int RequestPage);
        int NumberOfAccountsByRole(string roleName);
    }
}
