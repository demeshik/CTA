using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CTA.Models;
using CTA.ViewModels;
using AutoMapper;
using CTA.DTO;

namespace CTA.Controllers.api
{
    [Route("api/[controller]")]
    public class SessionController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> loginManager;
        private readonly RoleManager<Role> roleManager;

        public SessionController(UserManager<User> _userManager, SignInManager<User> _loginManager, RoleManager<Role> _roleManager)
        {
            userManager = _userManager;
            loginManager = _loginManager;
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

        [HttpPost]
        [Route("")]
        public ActionResult Register(RegisterUserModel user)
        {
            if(ModelState.IsValid)
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

                if(result.Succeeded)
                {
                    if(!roleManager.RoleExistsAsync("User").Result)
                    {
                        Role role = new Role()
                        {
                            Name = "User",
                            Description = "Simple user"
                        };
                        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                        if(!roleResult.Succeeded)
                        {
                            //ModelState.AddModelError("", "Error while creating role!");
                            return SendBad(roleResult.Errors);
                        }
                    }
                    userManager.AddToRoleAsync(_user, "User").Wait();
                    return Created("",Json(_user));
                }
            }
            return SendBad((new IdentityError { Code = "Error Model", Description = "Error in user definition" }));
        }

        [HttpPost]
        [Route("Authorization")]
        public ActionResult Login(LoginUserModel user)
        {
            if(ModelState.IsValid)
            {
                var result = loginManager.PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, false).Result;
                if(result.Succeeded)
                {
                    User _user = userManager.FindByNameAsync(user.UserName).Result;
                    Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());
                    UserDTO resUser = Mapper.Map<User, UserDTO>(_user);
                    return Ok(Json(resUser));
                }
            }
            return SendBad(new IdentityError { Code = "Error model", Description = "Error in login model" });
        }

        [HttpDelete]
        public IActionResult LogOff()
        {
            loginManager.SignOutAsync().Wait();
            return Ok(Json(new { status = "OK" }));
        }

    }
}
