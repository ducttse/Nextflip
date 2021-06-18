﻿using Nextflip.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IUserManagerManagementService
    {
        IEnumerable<Account> GetAllAccounts();
        IEnumerable<Account> GetAccountListByEmail(string searchValue,  string roleName, int RowsOnPage, int RequestPage);
        int NumberOfAccountsBySearching(string searchValue, string roleName);
        int NumberOfAccounts();
        int NumberOfAccountsByRole(string roleName);
        IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, int RowsOnPage, int RequestPage);
//        IEnumerable<Account> GetAllActiveAccounts();
//        IEnumerable<Account> GetAllInactiveAccounts();

    }
}
