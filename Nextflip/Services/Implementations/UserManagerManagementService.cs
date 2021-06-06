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
        public IEnumerable<Account> GetAccountListByEmail(string searchValue) =>
            GetAccountListByEmail(searchValue);

    }
}
