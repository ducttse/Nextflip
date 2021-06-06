using Microsoft.AspNetCore.Mvc;
using Nextflip.Models.account;
using Nextflip.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    public class UserManagerManagement : ControllerBase
    {
        public JsonResult GetAccountListByEmail(UserManagerManagementService userManagerManagementService, [FromForm] string searchValue)
        {
            IEnumerable<Account> accounts = (List<Account>)userManagerManagementService.GetAccountListByEmail(searchValue);
            return new JsonResult(accounts);
        }
    }
}
