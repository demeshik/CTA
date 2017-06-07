using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CTA.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using CTA.DTO;
using CTA.Services;
using CloudinaryDotNet;

namespace CTA.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICloudInterface cloud;
        public AccountController(UserManager<User> _userManager, ICloudInterface _cloud)
        {
            userManager = _userManager;
            cloud = _cloud;
        }

        [Authorize]
        public IActionResult Index()
        {
            var currentUser = userManager.FindByNameAsync(User.Identity.Name).Result;

            Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());

            var tupel = new Tuple<UserDTO, Cloudinary>(Mapper.Map<UserDTO>(currentUser),cloud.Configuration());

            return View(tupel);
        }
    }
}
