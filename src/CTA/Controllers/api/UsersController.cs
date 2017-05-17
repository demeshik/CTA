﻿using System;
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
using Microsoft.AspNetCore.Authorization;

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
            return BadRequest(new { status = "Error", description = identityError.Description });
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
            return BadRequest(new { status = "Error", description = ErrorMessage });
        }

        [HttpGet]
        public IEnumerable<UserDTO> GetAll()
        {
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            var users = Mapper.Map<IQueryable<User>, IEnumerable<UserDTO>>(userManager.Users);
            return users;
        }

        [HttpGet("{key}")]
        public UserDTO GetUserById(string key)
        {
            int _id;
            Models.User _user = null;
            if (int.TryParse(key, out _id)) 
                _user = userManager.FindByIdAsync(key).Result;
            else
                _user = userManager.FindByNameAsync(key).Result;
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            return Mapper.Map<User, UserDTO>(_user);
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
                    Name = user.Name,
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
                    return Created($"/api/users/{_user.UserName}", Mapper.Map<User, UserDTO>(_user));
                }
                else
                    return SendBad(result.Errors);
            }
            return SendBad((new IdentityError { Code = "Error Model", Description = "Error in user definition" }));
        }

        [HttpPut]
        [Authorize]
        public ActionResult Update(RegisterUserModel user)
        {
            
        }

        [HttpDelete]
        [Authorize(Roles="Admin")]
        public ActionResult Delete(string id)
        {
            IdentityResult result = null;
            User user = userManager.FindByIdAsync(id).Result;
            if (user != null)
                result = userManager.DeleteAsync(user).Result;
            else
                return NotFound();
            if (result.Succeeded)
            {
                return Ok(new { status = "OK" });
            }
            else
                return StatusCode(500, new { status = "Error", description = "Error has occured during removal of the user" });
        }
    }
}
