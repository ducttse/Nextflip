﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Services.Interfaces;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterAccount : ControllerBase
    {
        private readonly ILogger _logger;
        public RegisterAccount(ILogger<RegisterAccount> logger)
        {
            _logger = logger;
        }


        [Route("Register")]
        [HttpPost]
        public IActionResult Register([FromServices] IAccountService accountService, [FromBody]AccountRegisterForm form)
        {
            bool isValid = true;
            RegisterError error = new RegisterError();
            try
            {
                if(form.UserEmail == null || form.UserEmail.Trim().Length == 0)
                {
                    isValid = false;
                    error.EmailError = "Error empty email !";
                }
                if (form.Fullname == null || form.Fullname.Trim().Length == 0 || form.Fullname.Trim().Length > 50)
                {
                    isValid = false;
                    error.FullnameError = "Invalid fullname";
                }
                if (form.Password == null || form.Password.Length < 8 || form.Password.Length > 32)
                {
                    isValid = false;
                    error.PasswordError = "Invalid Password ! Password length must range from 8 - 32 character !";
                }
                if (!form.Password.Equals(form.ConfirmPassword))
                {
                    isValid = false;
                    error.ConfirmError = "Password and Confirm Password did not match !";
                }
                DateTime date;
                bool isValidDate = DateTime.TryParse(form.DateOfBirth, out date);
                if (isValidDate == false || date.Year >= (DateTime.Now.Year - 8))
                {
                    isValid = false;
                    error.DateOfBirthError = "Date of birth is Invalid";
                }
                if (!isValid) return new JsonResult(error);
                string result = accountService.RegisterAccount(form.UserEmail.ToLower(), form.Password, form.Fullname.Trim(), date, form.PictureURL);
                if (result == null) return new JsonResult(new { Message = "An Error Occurred ! Please try again" });
                return new JsonResult(new { Message = result });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("RegisterAccount/Register: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }
        [Route("CheckEmail")]
        [HttpPost]
        public IActionResult CheckEmail([FromServices] IAccountService accountService, [FromBody] AccountRegisterForm form)
        {
            try
            {
                if (EmailUtil.IsValidEmail(form.UserEmail)) return new JsonResult(new { Message = "Invalid email format" });
                if (accountService.IsExistedEmail(form.UserEmail)) return new JsonResult(new { Message = "Email has already existed exist!" });
                return new JsonResult(new { Message = "Valid" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("RegisterAccount/CheckEmail: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }
    }
    public partial class AccountRegisterForm
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Fullname { get; set; }
        public string DateOfBirth { get; set; }
        public string PictureURL { get; set; }
    }

    public partial class RegisterError
    {
        public string EmailError { get; set; }
        public string FullnameError { get; set; }
        public string PasswordError { get; set; }
        public string ConfirmError { get; set; }
        public string DateOfBirthError { get; set; }
    }
}
