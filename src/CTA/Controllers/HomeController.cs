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
            Account account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            Cloudinary cloud = new Cloudinary(account);
            return View(cloud);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
