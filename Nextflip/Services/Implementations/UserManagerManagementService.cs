using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.account;
using Nextflip.Services.Interfaces;

namespace Nextflip.Services.Implementations
{
    public class UserManagerManagementService : IUserManagerManagementService
    {
        private IAccountDAO _accountDao;
        public UserManagerManagementService(IAccountDAO accountDao) => _accountDao = accountDao;
        public IEnumerable<Account> GetAllAccounts() => _accountDao.GetAllAccounts();
        public IEnumerable<Account> GetAccountListByEmailFilterRoleStatus(string searchValue, string roleName, string status, int RowsOnPage, int RequestPage) 
                => _accountDao.GetAccountListByEmailFilterRoleStatus(searchValue, roleName, status, RowsOnPage, RequestPage);
        public int NumberOfAccountsBySearchingFilterRoleStatus(string searchValue, string roleName, string status) => _accountDao.NumberOfAccountsBySearchingFilterRoleStatus(searchValue, roleName, status);
        public int NumberOfAccountsBySearching(string searchValue) => _accountDao.NumberOfAccountsBySearching(searchValue);
        public int NumberOfAccountsByRoleAndStatus(string roleName, string status) => _accountDao.NumberOfAccountsByRoleAndStatus(roleName, status);
        public IEnumerable<Account> GetAccountsListByRoleAccordingRequest(string roleName, string status, int RowsOnPage, int RequestPage)
                => _accountDao.GetAccountsListByRoleAccordingRequest(roleName, status, RowsOnPage, RequestPage);

        //        public IEnumerable<Account> GetAllActiveAccounts() => _accountDao.GetAllActiveAccounts();
        //        public IEnumerable<Account> GetAllInactiveAccounts() => _accountDao.GetAllInactiveAccounts();
        public IEnumerable<Account> GetAccountListByEmail(string searchValue, int RowsOnPage, int RequestPage)
            => _accountDao.GetAccountListByEmail(searchValue, RowsOnPage, RequestPage);

        public IEnumerable<Account> GetAccountListByEmailFilterRole(string searchValue, string roleName, int RowsOnPage, int RequestPage)
                => _accountDao.GetAccountListByEmailFilterRole(searchValue, roleName, RowsOnPage, RequestPage);
        public int NumberOfAccountsBySearchingFilterRole(string searchValue, string roleName)
            => _accountDao.NumberOfAccountsBySearchingFilterRole(searchValue, roleName);

        public IEnumerable<Account> GetAccountsListOnlyByRole(string roleName, int RowsOnPage, int RequestPage)
                => _accountDao.GetAccountsListOnlyByRole(roleName, RowsOnPage, RequestPage);

        public int NumberOfAccountsByRole(string roleName) => _accountDao.NumberOfAccountsByRole(roleName);
        public bool ChangeAccountStatus(string userID, string note) => _accountDao.ChangeAccountStatus(userID, note);
    }
}
