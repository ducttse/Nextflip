using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Controllers
{
    public class Example : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
