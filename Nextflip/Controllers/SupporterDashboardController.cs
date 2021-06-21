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
        public IActionResult Index([FromBody] Request request)
        {
            try
            {
                ViewBag.body = request;
                return View();
            }
            catch(Exception e)
            {
                _logger.LogInformation("SupporterDashboardController/Index: " + e.Message);
                return View("Error");
            }
        }
        public IActionResult Detail(string id, [FromBody]Request request)
        {
            try
            {
                ViewBag.ticketId = id;
                ViewBag.body = request;
                return View();
            }
            catch(Exception e)
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
