﻿using Microsoft.AspNetCore.Mvc;
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
        [Route("api/UserManagerManagement/{searchValue}")]
        public JsonResult GetAccountListByEmail([FromServices] IUserManagerManagementService userManagerManagementService, string searchValue)
        {
            IEnumerable<Account> accounts = userManagerManagementService.GetAccountListByEmail(searchValue);
            Debug.WriteLine("FLAG");
            return new JsonResult(accounts);
        }
    }
}
