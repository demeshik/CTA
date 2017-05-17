using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CTA.ViewModels;
using CTA.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using CTA.DTO;

namespace CTA.Controllers.api
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UsersController(UserManager<User> _userManager, RoleManager<Role> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        private ActionResult SendBad(IdentityError identityError)
        {
            return BadRequest(Json(new { status = "Error", description = identityError.Description }));
        }
        private ActionResult SendBad(IEnumerable<IdentityError> Errors)
        {
            string ErrorMessage = string.Empty;
            foreach (var error in Errors)
            {
                ErrorMessage += error.Code + ";";
            }
            ErrorMessage.Remove(ErrorMessage.Length - 1, 1);
            ErrorMessage += ".";
            return BadRequest(Json(new { status = "Error", description = ErrorMessage }));
        }

        [HttpGet]
        public IEnumerable<UserDTO> GetAll()
        {
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            var users = Mapper.Map<IQueryable<User>, IEnumerable<UserDTO>>(userManager.Users);
            return users;
        }

        [HttpGet("{id}")]
        public UserDTO GetUser(string id)
        {
            User user = userManager.FindByIdAsync(id).Result;
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            return Mapper.Map<User, UserDTO>(user);
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel user)
        {
            if (ModelState.IsValid)
            {
                User _user = new User()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Country = user.Country,
                    City = user.City,
                    CreditCard = user.CreditCard
                };
                IdentityResult result = userManager.CreateAsync(_user, user.Password).Result;

                if (result.Succeeded)
                {
                    if (!roleManager.RoleExistsAsync("User").Result)
                    {
                        Role role = new Role()
                        {
                            Name = "User",
                            Description = "Simple user"
                        };
                        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            //ModelState.AddModelError("", "Error while creating role!");
                            return SendBad(roleResult.Errors);
                        }
                    }
                    userManager.AddToRoleAsync(_user, "User").Wait();
                    return Created("", Json(_user));
                }
            }
            return SendBad((new IdentityError { Code = "Error Model", Description = "Error in user definition" }));
        }
    }
}
