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
            try
            {
                if (accountService.IsExistedEmail(form.UserEmail)) return new JsonResult(new { Message = "Email has already existed exist!" });
                string result = accountService.RegisterAccount(form.UserEmail, form.GoogleID, form.GoogleEmail, form.Password, form.Fullname, form.DateOfBirth);
                if (result == null) return new JsonResult(new { Message = "An Error Occurred" });
                return new JsonResult(new { Message = result });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("RegisterAccount/Register: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }
    }
    public partial class AccountRegisterForm
    {
        public string UserEmail { get; set; }
        public string GoogleID { get; set; }
        public string GoogleEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Fullname { get; set; }
        public string DateOfBirth { get; set; }
    }
}
