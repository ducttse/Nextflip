﻿using Nextflip.utils;
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
using Nextflip.Models.subscription;
using System.Text.RegularExpressions;
using Nextflip.Models;

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
                return new JsonResult(new
                {
                    message = ex.Message
                });
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
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }


        public class Requesta
        {
            public string SearchValue { get; set; }
            public string RoleName { get; set; }
            public string Status { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
            public string UserID { get; set; }
            public string Note { get; set; }
        }

        //Get all + filter
        [HttpPost]
        [Route("GetAccountsListByRoleAccordingRequest")]
        public JsonResult GetAccountsListByRoleAccordingRequest([FromServices] IUserManagerManagementService userManagerManagementService,
                                [FromBody] Requesta request)
        {
            try
            {
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountsListByRoleAccordingRequest(request.RoleName, request.Status, request.RowsOnPage, request.RequestPage);
                int count = userManagerManagementService.NumberOfAccountsByRoleAndStatus(request.RoleName, request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsListByRoleAccordingRequest: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        //Get all
        [HttpPost]
        [Route("GetAccountsListOnlyByRole")]
        public JsonResult GetAccountsListOnlyByRole([FromServices] IUserManagerManagementService userManagerManagementService,
                                [FromBody] Requesta request)
        {
            try
            {
                IEnumerable<Account> accounts = userManagerManagementService.GetAccountsListOnlyByRole(request.RoleName, request.RowsOnPage, request.RequestPage);
                int count = userManagerManagementService.NumberOfAccountsByRole(request.RoleName);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsListByRoleAccordingRequest: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }


        //Search + Filter Role + Status
        [Route("GetAccountListByEmailFilterRoleStatus")]
        public JsonResult GetAccountListByEmailFilterRoleStatus([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] Requesta request)
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
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountListByEmail: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        //Search
        [Route("GetAccountListByEmail")]
        public JsonResult GetAccountListByEmail([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] Requesta request)
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
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountListByEmail: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        // Search + Filter Role
        [Route("GetAccountListByEmailFilterRole")]
        public JsonResult GetAccountListByEmailFilterRole([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] Requesta request)
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
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountListByEmail: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("InactiveAccount")]
        public JsonResult InactiveAccount([FromServices] IUserManagerManagementService userManagerManagementService, [FromForm] Requesta request)
        {
            try
            {
                bool result = userManagerManagementService.InactiveAccount(request.UserID, request.Note);
                var message1 = new
                {
                    message = "success"
                };
                var message2 = new
                {
                    message = "fail"
                };
                if (result) return new JsonResult(message1); else return new JsonResult(message2);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Inactive an account: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("ActiveAccount")]
        public JsonResult ActiveAccount([FromServices] IUserManagerManagementService userManagerManagementService, [FromForm] Requesta request)
        {
            try
            {
                bool result = userManagerManagementService.ActiveAccount(request.UserID);
                var message1 = new
                {
                    message = "success"
                };
                var message2 = new
                {
                    message = "fail"
                };
                if (result) return new JsonResult(message1); else return new JsonResult(message2);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Active an account: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("ReasonInactived")]
        public JsonResult ReasonInactived([FromServices] IUserManagerManagementService userManagerManagementService, [FromBody] Requesta request)
        {
            try
            {
                string note = userManagerManagementService.GetDetailOfInactiveAccount(request.UserID).note;
                var message = new
                {
                    reason = note
                };
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Active an account: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("IsValidEmail/{email}")]
        public IActionResult IsValidEmail([FromServices] IUserManagerManagementService userManagerManagementService,
                                string email)
        {
            string message = "";
            try
            {
                if (EmailUtil.IsValidEmail(email) == false)
                {
                    message = "Email is invalid format";
                }
                else if (userManagerManagementService.IsExistedEmail(email))
                {
                    message = "Email is existed";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("IsValidEmail: " + ex.Message);
                message = ex.Message;
            }
            return new JsonResult( new NotificationObject { message = message});
        }

        [Route("CreateStaff")]
        [HttpPost]
        public IActionResult CreateStaff([FromServices] IUserManagerManagementService userManagerManagementService,
                                        [FromBody] Account account)
        {
            NotificationObject noti = new NotificationObject();
            try
            {
                bool isValid = true;
                if (account.fullname.Trim() == string.Empty)
                {
                    noti.nameErr = "Full name must not be empty";
                    isValid = false;
                }
                if (EmailUtil.IsValidEmail(account.userEmail) == false)
                {
                    noti.emailErr = "Email is invalid format";
                    isValid = false;
                }
                else if (userManagerManagementService.IsExistedEmail(account.userEmail))
                {
                    noti.emailErr = "Email is existed";
                    isValid = false;
                }
                if (account.dateOfBirth == null) {
                    noti.dateTimeErr = "Date of birth is Invalid";
                    isValid = false;
                }
                bool result = userManagerManagementService.AddNewStaff(account);
                if (isValid == true && result == true) noti.message = "Success";
                else noti.message = "Fail";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CreateStaff: " + ex.Message);
            }
            return new JsonResult(noti);
        }

        [Route("GetSubscriptionByUserID")]
        [HttpPost]
        public IActionResult GetSubscriptionByUserID([FromServices] IUserManagerManagementService userManagerManagementService,
                                            [FromBody] Account user)
        {
           
            try
            {
                Subsciption subsciption = userManagerManagementService.GetSubsciptionByUserID(user.userID);
                return new JsonResult(subsciption);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetSubscriptionByUserID: " + ex.Message);
                return new JsonResult("error occur");
            }
        }
        [Route("UpdateExpiredDate")]
        [HttpPost]
        public IActionResult UpdateExpiredDate([FromServices] IUserManagerManagementService userManagerManagementService,
                                                    [FromBody] Subsciption subscript)
        {
            NotificationObject noti = new NotificationObject ();
            try
            {
                bool isValid = true;
                subscript.StartDate = DateTime.Now;
                if (subscript.EndDate == null || subscript.StartDate.CompareTo(subscript.EndDate) > 0)
                {
                    noti.dateTimeErr = "Date is invalid";
                    isValid = false;
                }
                bool result = userManagerManagementService.UpdateExpiredDate(subscript);
                if (isValid && result == true) noti.message = "Success";
                else noti.message = "Fail";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("UpdateExpiredDate: " + ex.Message);
            }
            return new JsonResult(noti);
        }

        [Route("EditStaffInfo")]
        [HttpPost]
        public IActionResult EditStaffInfo([FromServices] IUserManagerManagementService userManagerManagementService,
                                            [FromBody] Account staffInfo)
        {
            NotificationObject noti = new NotificationObject ();
            try
            {
                bool isValid = true;
                if (staffInfo.fullname.Trim() == string.Empty)
                {
                    noti.nameErr = "Full name must not be empty";
                    isValid = false;
                }
                if (staffInfo.dateOfBirth == null)
                {
                    noti.dateTimeErr = "Date of birth is Invalid";
                    isValid = false;
                }
                bool result = userManagerManagementService.UpdateStaffInfo(staffInfo); 
                if (isValid == true && result == true) noti.message = "Success";
                else noti.message = "Fail";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("EditStaffInfo: " + ex.Message);
            }
            return new JsonResult(noti);
        }
        [Route("IsSubscribedUser")]
        [HttpPost]
        public IActionResult IsSubscribedUser([FromServices] IUserManagerManagementService userManagerManagementService,
                                    [FromBody] Account user)
        {
            bool isSubscribedUser = false;
            try
            {
                isSubscribedUser = userManagerManagementService.IsSubscribedUser(user.userID);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("IsSubscribedUser: " + ex.Message);
            }
            return new JsonResult(isSubscribedUser);
        }
        [Route("GetUserProfile")]
        [HttpPost]
        public IActionResult GetUserProfile([FromServices] IUserManagerManagementService userManagerManagementService,
                                    [FromBody] Account _account)
        {
            Account account = null;
            try
            {
                    account = userManagerManagementService.GetAccountByID(_account.userID);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetProfile: " + ex.Message);
            }
            return new JsonResult(account);
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
