using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CTA.Models;
using CTA.ViewModels;

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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserModel user)
        {
            if(ModelState.IsValid)
            {
                User _user = new User();
                _user.UserName = user.UserName;
                _user.Email = user.Email;
                _user.Country = user.Country;
                _user.City = user.City;
                _user.CreditCard = user.CreditCard;

                IdentityResult result = userManager.CreateAsync(_user, user.Password).Result;

                if(result.Succeeded)
                {
                    if(!roleManager.RoleExistsAsync("User").Result)
                    {
                        Role role = new Role();
                        role.Description = "Simple user";
                        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                        if(!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Error while creating role!");
                            return Json(new { status = "Error", description = "Error while creating role" });
                        }
                    }
                    userManager.AddToRoleAsync(_user, "User").Wait();
                    return Created("",Json(_user));
                }
            }
            return BadRequest(new { status = "Error", description = "Error in model" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserModel user)
        {
            if(ModelState.IsValid)
            {
                var result = loginManager.PasswordSignInAsync(user.UserName, user.Password, user.RememberMe, false).Result;
                if(result.Succeeded)
                {
                    return Ok(Json(new { status = "OK" }));
                }
            }
            return BadRequest(Json(new { status = "Error", description = "Error parameters" }));
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            loginManager.SignOutAsync().Wait();
            return Ok(Json(new { status = "OK" }));
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
