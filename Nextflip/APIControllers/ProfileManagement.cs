﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

                bool result = accountService.ChangeProfile(profile.UserID, profile.UserEmail, profile.Fullname, profile.DateOfBirth, profile.PictureURL);
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
}
public partial class Profile
    {
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public string GoogleID { get; set; }
        public string GoogleEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Fullname { get; set; }
        public string DateOfBirth { get; set; }
        public string PictureURL { get; set; }
    }
}