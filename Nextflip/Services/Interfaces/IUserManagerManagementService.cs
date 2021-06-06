using Nextflip.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IUserManagerManagementService
    {
        IEnumerable<Account> GetAccountListByEmail(string searchValue);
       // IEnumerable<Media> getListMedia(string searchValue);
    }
}
