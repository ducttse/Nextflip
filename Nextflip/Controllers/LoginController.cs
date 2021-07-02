using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.account;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILogger _logger;
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();

        }

        [HttpPost]
        public IActionResult LoginWithEmail([FromServices] IAccountService accountService, [FromBody] LoginForm form)
        {
            try
            {
                if (form.Email.Trim().Length == 0 || form.Password.Trim().Length == 0)
                {
                    ViewBag.Error = "Please enter Email and Password!";
                    return RedirectToAction("Index");
                }
                else if (Regex.IsMatch(form.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    ViewBag.Error = "Incorrect email syntax";
                    return RedirectToAction("Index");
                }
                else if (!accountService.IsExistedEmail(form.Email))
                {
                    ViewBag.Error = "Email does not exist!";
                    return RedirectToAction("Index");
                }
                else
                {
                    Account account = accountService.Login(form.Email, form.Password);
                    if(account == null)
                    {
                        ViewBag.Error = "Incorrect Password";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (account.roleName.Equals("customer supporter")) return RedirectToAction("Index", "SupporterDashboard");
                    }
                }
                return RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("LoginController/LoginWithEmail: " + ex.Message);
                return RedirectToAction("Error", "Shared");
            }
        }
    }
    public partial class LoginForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string GoogleID { get; set; }
        public string GoogleEmail { get; set; }
    }
}
