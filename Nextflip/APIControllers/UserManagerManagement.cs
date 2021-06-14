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
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerManagement : ControllerBase
    {
        [Route("GetAccountListByEmail/{searchValue}")]
        public JsonResult GetAccountListByEmail([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] string searchValue)
        {
            IEnumerable<Account> accounts = userManagerManagementService.GetAccountListByEmail(searchValue);
            return new JsonResult(accounts);
        }

        [Route("GetAllAccounts")]
        public JsonResult GetAllAccounts([FromServices] IUserManagerManagementService userManagerManagementService)
        {
            IEnumerable<Account> accounts = userManagerManagementService.GetAllAccounts();
            return new JsonResult(accounts);
        }

        [Route("NumberOfAccounts")]
        public JsonResult NumberOfAccounts([FromServices] IUserManagerManagementService userManagerManagementService)
        {
            int count = userManagerManagementService.NumberOfAccounts();
            return new JsonResult(count);
        }
    }
}
