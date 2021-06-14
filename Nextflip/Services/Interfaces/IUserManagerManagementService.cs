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
        IEnumerable<Account> GetAccountsListAccordingRequest(int NumberOfPage, int RowOfPage, int RequestPage);

    }
}
