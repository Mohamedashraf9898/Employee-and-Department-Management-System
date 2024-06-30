using Demo.PL.Models;
using Demo.PL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IScopedService _Scoped1;
        private IScopedService _Scoped2;
        private ISingletoneService _singletone1;
        private ISingletoneService _singletone2;
        private ITransientService _transient1;
        private ITransientService _transient2;
        public HomeController(ILogger<HomeController> logger
                              , IScopedService scoped1
                              , IScopedService scoped2
                              , ISingletoneService singletone1
                              , ISingletoneService singletone2         
                              , ITransientService transient1
                              , ITransientService transient2
                              
            )
        {
            _Scoped1 = scoped1;
            _Scoped2 = scoped2;
            _singletone1 = singletone1;
            _singletone2 = singletone2;
            _transient1 = transient1;
            _transient2 = transient2;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        //Home/TestLifeTime
        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Scoped1::::::{_Scoped1.GetGuid()}");
            builder.Append($"Scoped2::::::{_Scoped2.GetGuid()}\n");
            builder.Append($"Singleton1::::::{_singletone1.GetGuid()}\n");
            builder.Append($"Singleton2::::::{_singletone2.GetGuid()}\n");
            builder.Append($"Transient1::::::{_transient1.GetGuid()}\n");
            builder.Append($"Transient2::::::{_transient2.GetGuid()}\n");
            return builder.ToString();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
