using Microsoft.AspNetCore.Mvc;
using Nextflip.Models.account;
using Nextflip.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Services.Interfaces;

namespace Nextflip.APIControllers
{
    public class UserManagerManagement : ControllerBase
    {
        [Route("api/UserManagerManagement/")]
        public JsonResult GetAccountListByEmail([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] string searchValue)
        {
            IEnumerable<Account> accounts = userManagerManagementService.GetAccountListByEmail(searchValue);
            Debug.WriteLine("FLAG");
            return new JsonResult(accounts);
        }

        public JsonResult GetAllAccounts([FromServices] IUserManagerManagementService userManagerManagementService)
        {
            IEnumerable<Account> accounts = userManagerManagementService.GetAllAccounts();
            Debug.WriteLine("FLAG");
            return new JsonResult(accounts);
        }
    }
}
