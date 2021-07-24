using Nextflip.utils;
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

        public partial class JsonAccount
        {
            public string userId { get; set; }
            public string userEmail { get; set; }
            public string roleName { get; set; }
            public string fullname { get; set; }
            public string dateOfBirth { get; set; }
        }
        [Route("CreateStaff")]
        [HttpPost]
        public IActionResult CreateStaff([FromServices] IUserManagerManagementService userManagerManagementService,
                                        [FromBody] JsonAccount _staffInfo)
        {
            NotificationObject noti = new NotificationObject();
            try
            {
                bool isValid = true;
                if (_staffInfo.fullname.Trim() == string.Empty)
                {
                    noti.nameErr = "Full name must not be empty";
                    isValid = false;
                }
                if (EmailUtil.IsValidEmail(_staffInfo.userEmail) == false)
                {
                    noti.emailErr = "Email is invalid format";
                    isValid = false;
                }
                else if (userManagerManagementService.IsExistedEmail(_staffInfo.userEmail))
                {
                    noti.emailErr = "Email is existed";
                    isValid = false;
                }
                DateTime date;
                bool isValidDate = DateTime.TryParse(_staffInfo.dateOfBirth, out date);
                if (isValidDate == false) {
                    noti.dateTimeErr = "Date of birth is Invalid";
                    isValid = false;
                }
                if (isValid == true)
                {
                    userManagerManagementService.AddNewStaff(new Account
                    {
                        userEmail = _staffInfo.userEmail,
                        fullname = _staffInfo.fullname,
                        roleName = _staffInfo.roleName,
                        dateOfBirth = date

                    });
                    noti.message = "Success";
                }
                else noti.message = "Fail";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CreateStaff: " + ex.Message);
            }
            return new JsonResult(noti);
        }

        public partial class JsonSubscription
        {
            public string userID { get; set; }
            public string EndDate { get; set; }
        }
        [Route("UpdateExpiredDate")]
        [HttpPost]
        public IActionResult UpdateExpiredDate([FromServices] IUserManagerManagementService userManagerManagementService,
                                                    [FromBody] JsonSubscription _subscript)
        {
            NotificationObject noti = new NotificationObject ();
            try
            {
                DateTime endDate;
                bool isValid = true;
                bool isValidDate = DateTime.TryParse(_subscript.EndDate, out endDate);
                if ( isValidDate == false || DateTime.Now.CompareTo(endDate) > 0)
                {
                    noti.dateTimeErr = "Date is invalid";
                    isValid = false;
                }
                if (isValid)
                {
                    userManagerManagementService.UpdateExpiredDate( new Subscription
                    {
                        UserID = _subscript.userID,
                        StartDate = DateTime.Now,
                        EndDate = endDate
                    });
                    noti.message = "Success";
                }
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
                                            [FromBody] JsonAccount _staffInfo)
        {
            NotificationObject noti = new NotificationObject ();
            try
            {
                bool isValid = true;
                if (_staffInfo.fullname.Trim() == string.Empty)
                {
                    noti.nameErr = "Full name must not be empty";
                    isValid = false;
                }
                DateTime date;
                bool isValidDate = DateTime.TryParse(_staffInfo.dateOfBirth, out date);
                if (isValidDate == false)
                {
                    noti.dateTimeErr = "Date of birth is Invalid";
                    isValid = false;
                }
                if (isValid == true)
                {
                    userManagerManagementService.UpdateStaffInfo(new Account
                    {
                        userID = _staffInfo.userId,
                        fullname = _staffInfo.fullname,
                        roleName = _staffInfo.roleName,
                        dateOfBirth = date

                    });
                    noti.message = "Success";
                }
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

        public partial class ProfileForEditing
        {
            public string userID { get; set; }
            public string userEmail { get; set; }
            public string roleName { get; set; }
            public string fullname { get; set; }
            public DateTime dateOfBirth { get; set; }
            public Subscription expiration { get; set; }
        }
        [Route("GetUserProfile")]
        [HttpPost]
        public IActionResult GetUserProfile([FromServices] IUserManagerManagementService userManagerManagementService,
                                    [FromBody] Account _account)
        {
            ProfileForEditing profile = null;
            try
            {
                Account account = userManagerManagementService.GetAccountByID(_account.userID);
                Subscription subscription = userManagerManagementService.GetSubsciptionByUserID(_account.userID);
                profile = new ProfileForEditing
                {
                    userID = account.userID,
                    userEmail = account.userEmail,
                    roleName = account.roleName,
                    fullname = account.fullname,
                    expiration = subscription
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetProfile: " + ex.Message);
            }
            return new JsonResult(profile);
        }
        public partial class SubscriptionRequest
        {
            public int Rows { get; set; } = 10;
            public int Page { get; set; } = 1;
            public string Status { get; set; } = "%%";
            public string UserEmail { get; set; }
        }

        [Route("GetSubscriptions")]
        [HttpPost]
        public IActionResult GetSubscriptions([FromServices] IUserManagerManagementService userManagerManagementService,
                                                [FromBody] SubscriptionRequest request)
        {
            IEnumerable<object> subscriptions = null;
            try
            {
                 subscriptions = userManagerManagementService.GetSubscriptions(request.Rows, request.Page, request.Status);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetSubscriptions: " + ex.Message);
                return new JsonResult("error"+ex.Message);
            }
            return new JsonResult(subscriptions);
        }

        [Route("GetSubscriptionsByUserEmail")]
        [HttpPost]
        public IActionResult GetSubscriptionsByUserEmail([FromServices] IUserManagerManagementService userManagerManagementService,
                                                            [FromBody] SubscriptionRequest request)
        {
            IEnumerable<object> subscriptions = null;
            try
            {
                if (request.UserEmail.Trim() != string.Empty)
                {
                    subscriptions = userManagerManagementService.GetSubscriptionsByUserEmail(request.UserEmail, request.Rows, request.Page, request.Status);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetSubscriptionsByUserEmail: " + ex.Message);
                return new JsonResult("error" + ex.Message );
            }
            return new JsonResult(subscriptions);
        }

        [Route("RefundSubscription")]
        [HttpPost]
        public IActionResult RefundSubscription([FromServices] IUserManagerManagementService userManagerManagementService,
                                                [FromBody] Subscription subscription)
        {
            try
            {
                var _subscription = userManagerManagementService.GetSubsciptionByUserID(subscription.UserID);
                if (_subscription != null && subscription.SubscriptionID.Equals(_subscription.SubscriptionID))
                {
                    if (_subscription.StartDate > DateTime.Now)
                    {
                        bool result = userManagerManagementService.RefundSubscription(subscription.SubscriptionID);
                        if (result == true) return new JsonResult(new { Message = "Success" });
                    }
                }
                return new JsonResult(new { Message = "Fail"});
            }
            catch (Exception ex)
            {
                _logger.LogInformation("RefundSubscription: " + ex.Message);
                return new JsonResult(new { Message = "Fail " +ex.Message } );
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
