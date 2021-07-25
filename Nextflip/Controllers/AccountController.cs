using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Nextflip.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();

        public IActionResult Register() => View();

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
            return RedirectToAction("Login", "Account");
        }

        [Route("ConfirmEmail/{userID}/{token}")]
        public IActionResult ConfirmEmail([FromServices] AccountService accountService, string userID, string token)
        {
            if (userID == null || token == null) return NotFound();
            string role = accountService.ConfirmEmail(userID, token);
            if (role == null) return NotFound();
            return View();
        }

    }
}
