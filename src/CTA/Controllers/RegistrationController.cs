using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CTA.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {

            Cloudinary cloud = new Cloudinary("cloudinary://699756615932382:m5MHlmJJkZmGa_H_7VPdYpo6JyA@djrazor308");
            return View(cloud);
        }
    }
}
