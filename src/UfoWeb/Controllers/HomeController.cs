using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UfoWeb.Clients;
using UfoWeb.Models;

namespace UfoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataHerbClient _dataHerbClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(DataHerbClient dataHerbClient, ILogger<HomeController> logger)
        {
            _dataHerbClient = dataHerbClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dataHerbClient.GetAsync();
            return View(new HomeViewModel(data));
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
