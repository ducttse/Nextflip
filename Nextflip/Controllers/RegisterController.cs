using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nextflip.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly ILogger _logger;
        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index() => View();
    }
}
