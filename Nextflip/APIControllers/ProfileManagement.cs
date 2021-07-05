using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.account;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            try
            {
                string userID = HttpContext.Session.GetString("ACCOUNT_ID");
                bool result = accountService.ChangeProfile(userID, profile.UserEmail, profile.Fullname, profile.DateOfBirth, profile.PictureURL);
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
                string userID = HttpContext.Session.GetString("ACCOUNT_ID");
                if(profile.Password.Length < 8 || profile.Password.Length > 32) return new JsonResult(new { Message = "Password length must be between 8 - 32 characters!" });
                if(!profile.Password.Equals(profile.ConfirmPassword)) return new JsonResult(new { Message = "Password and Confirm Password did not match!" });
                bool result = accountService.ChangePassword(userID, profile.Password);
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
        public IActionResult GetProfile([FromServices] IAccountService accountService)
        {
            try
            {
                string userID = HttpContext.Session.GetString("ACCOUNT_ID");
                Account account = accountService.GetProfile(userID);
                if (account == null) return new JsonResult(new { Message = "An error occured ! Please wait and try again !" });
                return new JsonResult(new Profile
                {
                    UserEmail = account.userEmail,
                    Fullname = account.fullname,
                    DateOfBirth = account.dateOfBirth.ToString(),
                    PictureURL = account.pictureURL
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
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Fullname { get; set; }
        public string DateOfBirth { get; set; }
        public string PictureURL { get; set; }
    }
}
