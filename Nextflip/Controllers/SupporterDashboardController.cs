using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nextflip.Controllers
{
    [Authorize(Policy = "customer supporter")]

    public class SupporterDashboardController : Controller
    {
        private readonly ILogger _logger;

        public SupporterDashboardController(ILogger<SupporterDashboardController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _logger.LogInformation("SupporterDashboardController/Index: " + e.Message);
                return View("Error");
            }
        }
        [HttpGet]
        public IActionResult Detail(string id)
        {
            try
            {
                ViewBag.ticketId = id;
                return View();
            }
            catch (Exception e)
            {
                _logger.LogInformation("SupporterDashboardController/Detail: " + e.Message);
                return View("Error");
            }
        }
    }

    public partial class Request
    {
        public int RowsOnPage { get; set; }
        public int RequestPage { get; set; }
        public string TopicName { get; set; }
        public string SearchValue { get; set; }
        public string Status { get; set; }
    }
}
