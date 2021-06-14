using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Nextflip.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index() => View();
    }
}