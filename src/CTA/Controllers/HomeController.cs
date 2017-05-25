using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using CTA.DTO;

namespace CTA.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<AppConfig> config;
        public HomeController(IOptions<AppConfig> _config)
        {
            config = _config;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
