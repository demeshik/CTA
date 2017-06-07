using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CTA.Services;

namespace CTA.Controllers
{
    [Route("[controller]")]
    public class RegistrationController : Controller
    {
        private readonly ICloudInterface _cloud;

        public RegistrationController(ICloudInterface cloud)
        {
            _cloud = cloud;
        }

        public IActionResult Index()
        { 
            return View(_cloud.Configuration());
        }
    }
}
