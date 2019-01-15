using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zh.MVC.Models;
using Zh.MVC.Services;

namespace Zh.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ZhService service;

        public HomeController(ZhService service)
        {
            this.service = service;
        }

        public IActionResult Index(HomeViewModel vm)
        {
            return View("", vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
