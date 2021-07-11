using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.account;
using Nextflip.Models.subscription;
using Nextflip.Services.Interfaces;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileManagement : ControllerBase
    {
        private readonly ILogger _logger;
        public ProfileManagement(ILogger<ProfileManagement> logger)
        {
            _logger = logger;
        }


        [Route("ChangeProfile")]
        [HttpPost]
        public IActionResult ChangeProfile([FromServices] IAccountService accountService, [FromBody] Profile profile)
        {
            bool isValid = true;
            UpdateError error = new UpdateError();
            try
            {
                if (profile.Fullname == null || profile.Fullname.Trim().Length == 0 || profile.Fullname.Trim().Length > 50)
                {
                    isValid = false;
                    error.FullnameError = "Invalid fullname";
                }
                DateTime date;
                bool isValidDate = DateTime.TryParse(profile.DateOfBirth, out date);
                if (isValidDate == false || date.Year >= (DateTime.Now.Year - 8))
                {
                    isValid = false;
                    error.DateOfBirthError = "Date of birth is Invalid";
                }
                if (!isValid) return new JsonResult(error);
                bool result = accountService.ChangeProfile(profile.UserID, profile.Fullname, date, profile.PictureURL);
                if(result) return new JsonResult(new { Message = true });
                return new JsonResult(new { Message = false });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ProfileManagement/ChangeProfile: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword([FromServices] IAccountService accountService, [FromBody] Profile profile)
        {
            try
            {
                if(profile.Password.Length < 8 || profile.Password.Length > 32) return new JsonResult(new { Message = "Password length must be between 8 - 32 characters!" });
                if(!profile.Password.Equals(profile.ConfirmPassword)) return new JsonResult(new { Message = "Password and Confirm Password did not match!" });
                bool result = accountService.ChangePassword(profile.UserID, profile.Password);
                if (result) return new JsonResult(new { Message = true });
                return new JsonResult(new { Message = false });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ProfileManagement/ChangePassword: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("GetProfile")]
        [HttpPost]
        public IActionResult GetProfile([FromServices] IAccountService accountService, [FromServices] ISubscriptionService subscriptionService, Profile profile)
        {
            try
            {
                Account account = accountService.GetProfile(profile.UserID);
                if (account == null) return new JsonResult(new { Message = "An error occured ! Please wait a moment and try again !" });
                Subscription subscription = subscriptionService.GetSubsciptionByUserID(profile.UserID);
                if (subscription == null) return new JsonResult( new
                {
                    UserEmail = account.userEmail,
                    Fullname = account.fullname,
                    DateOfBirth = account.dateOfBirth.ToString("yyyy-MM-dd"),
                    PictureURL = account.pictureURL
                });
                return new JsonResult(new
                {
                    UserEmail = account.userEmail,
                    Fullname = account.fullname,
                    DateOfBirth = account.dateOfBirth.ToString("yyyy-MM-dd"),
                    PictureURL = account.pictureURL,
                    SubscriptionID = subscription.SubscriptionID,
                    SubscriptionEndDate = subscription.EndDate.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ProfileManagement/ChangePassword: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

    }
    public partial class Profile
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Fullname { get; set; }
        public string DateOfBirth { get; set; }
        public string PictureURL { get; set; }
    }
    public partial class UpdateError
    {
        public string FullnameError { get; set; }
        public string DateOfBirthError { get; set; }
    }
}
