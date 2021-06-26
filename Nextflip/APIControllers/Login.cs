using Microsoft.AspNetCore.Http;
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
                if (!accountService.IsExistedEmail(form.Email)) return new JsonResult(new { Message = "Email does not exist!" });
                bool result = accountService.Login(form.Email, form.Password);
                if (result == false) return new JsonResult(new { Message = "Invalid userEmail or Password" });
                return new JsonResult(new { Message = true });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Login/LoginAccount: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        public partial class LoginForm
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
