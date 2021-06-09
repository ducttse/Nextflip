using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    public class SupporterDashboardController : Controller
    {
        private readonly ILogger _logger;

        public SupporterDashboardController(ILogger<SupporterDashboardController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch(Exception e)
            {
                _logger.LogInformation("SupporterDashboardController/Index: " + e.Message);
                return View("Error");
            }
        }

    }
}
