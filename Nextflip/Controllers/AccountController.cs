using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nextflip.Services.Implementations;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public IActionResult Login() => View();

        public IActionResult Register() => View();

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("Account/ConfirmEmail/{userID}/{token}")]
        public IActionResult ConfirmEmail([FromServices] IAccountService accountService, string userID, string token)
        {
            if (userID == null || token == null) return NotFound();
            string role = accountService.ConfirmEmail(userID, token);
            if (role == null) return NotFound();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword([FromServices] AccountService accountService, string userID, string token) => View();
    }
}
