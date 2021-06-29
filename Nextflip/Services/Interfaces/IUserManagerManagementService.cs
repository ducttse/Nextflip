﻿using Nextflip.Models.account;
using Nextflip.Models.subscription;
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
        bool InactiveAccount(string userID, string note);
        bool ActiveAccount(string userID);
        Account GetDetailOfInactiveAccount(string userID);
        bool AddNewStaff(Account account);
        bool IsExistedEmail(string email);
        bool UpdateExpiredDate(Subsciption subsciption);
        Account GetAccountByID(string userID);
        bool UpdateStaffInfo(Account account);
        Subsciption GetSubsciptionByUserID(string userID);
        bool IsSubscribedUser(string userID);
    }
}
