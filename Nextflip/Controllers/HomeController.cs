using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nextflip.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        public IActionResult Index() => View();
    }
}