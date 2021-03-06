using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.account;
using Nextflip.Models.paymentPlan;
using Nextflip.Models.subscription;
using Nextflip.Services.Interfaces;

namespace Nextflip.Services.Implementations
{
    public class UserManagerManagementService : IUserManagerManagementService
    {
        private IAccountDAO _accountDao;
        private ISubscriptionDAO _subscription;
        private IPaymentPlanDAO _paymentPlan;
        public UserManagerManagementService(IAccountDAO accountDao, ISubscriptionDAO subscription, IPaymentPlanDAO paymentPlan)
        {
            _accountDao = accountDao;
            _subscription = subscription;
            _paymentPlan = paymentPlan;
        }
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
        public bool InactiveAccount(string userID, string note) => _accountDao.InactiveAccount(userID, note);
        public bool ActiveAccount(string userID) => _accountDao.ActiveAccount(userID);
        public Account GetDetailOfInactiveAccount(string userID) => _accountDao.GetDetailOfInactiveAccount(userID);
        public bool AddNewStaff(Account account) => _accountDao.AddNewStaff(account);
        public bool IsExistedEmail(string email) => _accountDao.IsExistedEmail(email);
        public bool UpdateExpiredDate(Subscription subsciption) => _subscription.UpdateExpiredDate(subsciption);
        public Account GetAccountByID(string userID) => _accountDao.GetAccountByID(userID);
        public bool UpdateStaffInfo(Account account) => _accountDao.UpdateStaffInfo(account);
        public Subscription GetSubsciptionByUserID(string userID) => _subscription.GetSubsciptionByUserID(userID);
        public bool IsSubscribedUser(string userID) => _accountDao.IsSubscribedUser(userID);
        public Account GetAccountByEmail(string email) => _accountDao.GetAccountByEmail(email);
        public IEnumerable<object> GetSubscriptions(int rows, int page, string status) => _subscription.GetSubscriptions(rows,page,status);
        public IEnumerable<object> GetSubscriptionsByUserEmail(string userEmail, int rows, int page, string status) => _subscription.GetSubscriptionsByUserEmail(userEmail,rows,page,status);
        public int CountTotalResult(string userEmail, string status) => _subscription.CountTotalResult(userEmail, status);
        public bool RefundSubscription(string subscriptionID) => _subscription.RefundSubscription(subscriptionID);
        public void UpdatePaymentPlan(int updateFormDuration, decimal updateFormNewPrice)
        {
            _paymentPlan.UpdatePaymentPlan(updateFormDuration, updateFormNewPrice);
        }
    }
}
