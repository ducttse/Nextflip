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
        IEnumerable<Account> GetAccountListByEmail(string searchValue);
        int NumberOfAccounts();
        IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, int NumberOfPage, int RowsOnPage, int RequestPage);
//        IEnumerable<Account> GetAllActiveAccounts();
//        IEnumerable<Account> GetAllInactiveAccounts();

    }
}
