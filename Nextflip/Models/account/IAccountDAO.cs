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
        bool InactiveAccount(string userID, string note);
        int NumberOfAccountsBySearching(string searchValue);
        int NumberOfAccountsByRoleAndStatus(string roleName, string status);
        IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, string status, int RowsOnPage, int RequestPage);
        IEnumerable<Account> GetAccountListByEmail(string searchValue, int RowsOnPage, int RequestPage);

        IEnumerable<Account> GetAccountListByEmailFilterRole(string searchValue, string roleName, int RowsOnPage, int RequestPage);
        int NumberOfAccountsBySearchingFilterRole(string searchValue, string roleName);

        IEnumerable<Account> GetAccountsListOnlyByRole(string roleName, int RowsOnPage, int RequestPage);
        int NumberOfAccountsByRole(string roleName);
        Account GetDetailOfAccount(string userID);
        Account GetDetailOfInactiveAccount(string userID);
        bool ActiveAccount(string userID);
        // add update
        bool AddNewStaff(Account account);
        bool UpdateStaffInfo(Account account);
        Account GetAccountByID(string userID);
        Account GetAccountByEmail(string email);
        bool IsExistedEmail(string email);
        public bool ChangeProfile(string userID, string fullname, DateTime dateOfBirth, string pictureURL);
        public bool RegisterAnAccount(string userEmail, string password, string fullname, DateTime dateOfBirth, string pictureURL);
        public Account Login(string email, string password);
        bool IsSubscribedUser(string userID);
        public bool ChangePassword(string userID, string password);
        public Account CheckGoogleLogin(string googleID);
        public bool CheckWallet(string userID, double money);
        public bool PurchaseSubscription(string userID, double money);
    }

}