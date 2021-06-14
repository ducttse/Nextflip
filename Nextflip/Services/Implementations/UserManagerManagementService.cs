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
        public IEnumerable<Account> GetAccountListByEmail(string searchValue) => _accountDao.GetAccountListByEmail(searchValue);
        public int NumberOfAccounts() => _accountDao.NumberOfAccounts();
    }
}
