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

namespace CTA.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        public AccountController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var currentUser = userManager.FindByNameAsync(User.Identity.Name).Result;

            Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());
            return View(Mapper.Map<UserDTO>(currentUser));
        }
    }
}
