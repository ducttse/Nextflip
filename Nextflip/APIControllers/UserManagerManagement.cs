using Microsoft.AspNetCore.Mvc;
using Nextflip.Models.account;
using Nextflip.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerManagement : ControllerBase
    {
        private readonly ILogger _logger;

        public UserManagerManagement(ILogger<UserManagerManagement> logger)
        {
            _logger = logger;
        }

        [Route("GetAccountListByEmail/{searchValue}")]
        public JsonResult GetAccountListByEmail([FromServices] IUserManagerManagementService userManagerManagementService, string searchValue)
        {           
            try
            {
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountListByEmail(searchValue);
                return new JsonResult(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountListByEmail: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        [Route("GetAllAccounts")]
        public JsonResult GetAllAccounts([FromServices] IUserManagerManagementService userManagerManagementService)
        {
            try 
            {
            IEnumerable<Account> accounts = userManagerManagementService.GetAllAccounts();
            return new JsonResult(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAllAccounts: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        [Route("NumberOfAccounts")]
        public JsonResult NumberOfAccounts([FromServices] IUserManagerManagementService userManagerManagementService)
        {
            try
            {
            int count = userManagerManagementService.NumberOfAccounts();
            return new JsonResult(count);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("NumberOfAccounts: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        public class Request
        {
            public string roleName { get; set; }
            public int NumberOfPage { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

        [HttpPost]
        [Route("GetAccountsListByRoleAccordingRequest")]
        public JsonResult GetAccountsListByRoleAccordingRequest([FromServices] IUserManagerManagementService userManagerManagementService,
                                [FromBody] Request request)
        {
            try
            { 
            IEnumerable<Account> accounts = userManagerManagementService.GetAccountsListByRoleAccordingRequest(request.roleName, request.NumberOfPage, request.RowsOnPage, request.RequestPage);
            return new JsonResult(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsListByRoleAccordingRequest: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

/*        [Route("GetAllActiveAccounts")]
        public JsonResult GetAllActiveAccounts([FromServices] IUserManagerManagementService userManagerManagementService)
        {
            try
            {
                IEnumerable<Account> accounts = userManagerManagementService.GetAllActiveAccounts();
                return new JsonResult(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAllActiveAccounts: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        [Route("GetAllInactiveAccounts")]
        public JsonResult GetAllInactiveAccounts([FromServices] IUserManagerManagementService userManagerManagementService)
        {
            try
            {
                IEnumerable<Account> accounts = userManagerManagementService.GetAllInactiveAccounts();
                return new JsonResult(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAllInactiveAccounts: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }
*/
    }
}
