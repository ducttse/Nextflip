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
    public class Login : ControllerBase
    {
        private readonly ILogger _logger;
        public Login(ILogger<Login> logger)
        {
            _logger = logger;
        }


        [Route("LoginAccount")]
        [HttpPost]
        public IActionResult LoginAccount([FromServices] IAccountService accountService, [FromBody] LoginForm form)
        {
            try
            {
                if (form.Email.Trim().Length == 0 || form.Password.Trim().Length == 0) return new JsonResult(new { Message = "Please enter Email and Password!" });
                if (!accountService.IsExistedEmail(form.Email)) return new JsonResult(new { Message = "Email does not exist!" });
                Account result = accountService.Login(form.Email, form.Password);
                if (result == null) return new JsonResult(new { Message = "Invalid userEmail or Password" });
                return new JsonResult(new { Message = true });
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
                if (!accountService.IsExistedEmail(form.Email)) return new JsonResult(new { Message = "Email does not exist!" });
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
        public IActionResult LoginByGmail([FromServices] IAccountService accountService, [FromBody] LoginForm form)
        {
            try
            {
                if (accountService.CheckGoogleLogin(form.GoogleID, form.GoogleEmail)) return new JsonResult(new { Message = "Success" });
                return new JsonResult(new { Message = "Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Login/LoginByGmail: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

    }
}
