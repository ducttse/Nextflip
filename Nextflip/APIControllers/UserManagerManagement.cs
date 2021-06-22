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
using Newtonsoft.Json;
using Nextflip.Models.role;

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

        [Route("GetRoleNameList")]
        public JsonResult GetRoleNameList([FromServices] IRoleService roleService)
        {
            try
            {
                IEnumerable<Role> roles = roleService.GetRoleNameList();
                return new JsonResult(roles);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetRoleNameList: " + ex.Message);
                return new JsonResult("An error occur");
            }
        }


        public class Request
        {
            public string SearchValue { get; set; }
            public string RoleName { get; set; }
            public string Status { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

        //Get all + filter
        [HttpPost]
        [Route("GetAccountsListByRoleAccordingRequest")]
        public JsonResult GetAccountsListByRoleAccordingRequest([FromServices] IUserManagerManagementService userManagerManagementService,
                                [FromBody] Request request)
        {
            try
            {
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountsListByRoleAccordingRequest(request.RoleName, request.Status, request.RowsOnPage, request.RequestPage);
                int count = userManagerManagementService.NumberOfAccountsByRoleAndStatus(request.RoleName, request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsListByRoleAccordingRequest: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        //Get all
        [HttpPost]
        [Route("GetAccountsListOnlyByRole")]
        public JsonResult GetAccountsListOnlyByRole([FromServices] IUserManagerManagementService userManagerManagementService,
                                [FromBody] Request request)
        {
            try
            {
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountsListOnlyByRole(request.RoleName, request.RowsOnPage, request.RequestPage);
                int count = userManagerManagementService.NumberOfAccountsByRole(request.RoleName);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsListByRoleAccordingRequest: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }


        //Search + Filter Role + Status
        [Route("GetAccountListByEmailFilterRoleStatus")]
        public JsonResult GetAccountListByEmailFilterRoleStatus([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountListByEmailFilterRoleStatus(request.SearchValue.Trim(), request.RoleName, request.Status, request.RowsOnPage, request.RequestPage);
                int count = userManagerManagementService.NumberOfAccountsBySearchingFilterRoleStatus(request.SearchValue.Trim(), request.RoleName, request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountListByEmail: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        //Search
        [Route("GetAccountListByEmail")]
        public JsonResult GetAccountListByEmail([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountListByEmail(request.SearchValue.Trim(), request.RowsOnPage, request.RequestPage);
                int count = userManagerManagementService.NumberOfAccountsBySearching(request.SearchValue.Trim());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountListByEmail: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        // Search + Filter Role
        [Route("GetAccountListByEmailFilterRole")]
        public JsonResult GetAccountListByEmailFilterRole([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountListByEmailFilterRole(request.SearchValue.Trim(), request.RoleName, request.RowsOnPage, request.RequestPage);
                int count = userManagerManagementService.NumberOfAccountsBySearchingFilterRole(request.SearchValue.Trim(), request.RoleName);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountListByEmail: " + ex.Message);
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
