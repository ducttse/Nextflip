using Microsoft.AspNetCore.Mvc;
using Nextflip.Models.account;
using Nextflip.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    public class UserManagerManagement : Controller
    {
        public ActionResult GetAccountListByEmail(UserManagerManagementService userManagerManagementService,[FromForm] string searchValue)
        {
                IEnumerable<Account> accounts = (List<Account>)userManagerManagementService.GetAccountListByEmail(searchValue);
            return View(accounts);
        }
    }
}
