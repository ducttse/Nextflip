using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.account;
using Nextflip.Services.Interfaces;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        private readonly ILogger _logger;
        public Login(ILogger<Login> logger)
        {
            _logger = logger;
        }

        private async Task<bool> SignIn(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.userID),
                new Claim(ClaimTypes.Role, account.roleName),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return true;
        } 

        [Route("LoginAccount")]
        [HttpPost]
        public async Task<IActionResult> LoginAccount([FromServices] IAccountService accountService, [FromBody] LoginForm form)
        {
            try
            {
                if (form.Email == null || form.Email.Trim().Length == 0 || form.Password == null || form.Password.Trim().Length == 0) return new JsonResult(new { Message = "Please enter Email and Password !" });
                Account account = accountService.Login(form.Email.ToLower(), form.Password);
                if (account == null) return new JsonResult(new { Message = "Incorrect Password !" });
                string url = null;
                if (account.roleName.Equals("customer supporter")) url = "/SupporterDashboard/Index";
                else if (account.roleName.Equals("subscribed user")) url = "/SubcribedUserDashBoard/Index";
                else if (account.roleName.Equals("media editor")) url = "/EditorDashboard/Index";
                else if (account.roleName.Equals("user manager")) url = "/UserManagerManagement/Index";
                else if (account.roleName.Equals("media manager")) url = "/MediaManagerManagement/Index";
                var x = await SignIn(account);
                return new JsonResult(new { Message = true , URL = url, UserID = account.userID});
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Login/LoginAccount: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("CheckEmail")]
        [HttpPost]
        public IActionResult CheckEmail([FromServices] IAccountService accountService, [FromBody] LoginForm form)
        {
            try
            {
                if (!EmailUtil.IsValidEmail(form.Email)) return new JsonResult(new { Message = "Invalid Email !"});
                if (!accountService.IsExistedEmail(form.Email)) return new JsonResult(new { Message = "Email does not exist !" });
                return new JsonResult(new { Message = "Valid" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Login/CheckEmail: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("LoginByGmail")]
        [HttpPost]
        public async Task<IActionResult> LoginByGmail([FromServices] IAccountService accountService, [FromBody] LoginForm form)
        {
            string url = null;
            try
            {
                if (form.Email == null || form.Email.Trim().Length == 0 || !EmailUtil.IsValidEmail(form.Email)) return new JsonResult(new { Message = "Error ! Please try again !" });
                Account account = accountService.CheckGoogleLogin(form.Email);
                if (account == null) return new JsonResult(new { Message = "New Account", URL = "/Account/Register" });
                if (account.roleName.Equals("customer supporter")) url = "/SupporterDashboard/Index";
                else if (account.roleName.Equals("subscribed user")) url = "/SubcribedUserDashBoard/Index";
                else if (account.roleName.Equals("media editor")) url = "/EditorDashboard/Index";
                else if (account.roleName.Equals("user manager")) url = "/UserManagerManagement/Index";
                else if (account.roleName.Equals("media manager")) url = "/MediaManagerManagement/Index";
                var x = await SignIn(account);
                return new JsonResult(new { Message = true , URL = url, UserID = account.userID});
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Login/LoginByGmail: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }
    }
    public partial class LoginForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
