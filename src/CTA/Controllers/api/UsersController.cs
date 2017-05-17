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
using CTA.Context;
using Microsoft.EntityFrameworkCore;

namespace CTA.Controllers.api
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DBContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UsersController(UserManager<User> _userManager, RoleManager<Role> _roleManager, DBContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
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
        public UserDTO GetUserById(string id)
        {
            User user = userManager.FindByIdAsync(id).Result;
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            return Mapper.Map<User, UserDTO>(user);
        }

        [HttpGet("{username}")]
        public UserDTO GetUserByUsername(string username)
        {
            User user = userManager.FindByNameAsync(username).Result;
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            return Mapper.Map<User, UserDTO>(user);
        }

        [HttpGet("{id}/lots")]
        public IEnumerable<Lot> GetUserLots(string id)
        {
            User user = userManager.FindByIdAsync(id).Result;
            return user.Lots;
        }

        [HttpGet("{id}/bids")]
        public IEnumerable<Bid> GetUserBids(string id)
        {
            User user = userManager.FindByIdAsync(id).Result;
            return user.Bids;
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel user)
        {
            if (ModelState.IsValid)
            {
                User _user = new User()
                {
                    UserName = user.UserName,
                    Name = user.UserName,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
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
                            return SendBad(roleResult.Errors);
                        }
                    }
                    userManager.AddToRoleAsync(_user, "User").Wait();
                    Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
                    return Created($"/api/users/{_user.UserName}", Json(Mapper.Map<User, UserDTO>(_user)));
                }
            }
            return SendBad((new IdentityError { Code = "Error Model", Description = "Error in user definition" }));
        }
    }
}
