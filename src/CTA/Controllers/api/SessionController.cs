using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CTA.Models;
using CTA.ViewModels;
using AutoMapper;
using CTA.DTO;
using Microsoft.AspNetCore.Authorization;
using CTA.Utils;
using CTA.Repositories;

namespace CTA.Controllers.api
{
    [Route("api/[controller]")]
    public class SessionController : Controller
    {
        private readonly SignInManager<User> loginManager;
        private readonly ISignInManager signInManager;

        public SessionController(ISignInManager _signManager)
        {
            signInManager = _signManager;
            
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
        public ActionResult Login([FromBody]LoginUserModel user)
        {
            if (ModelState.IsValid)
            {
                var result = signInManager.Login(user.UserName, user.Password, user.RememberMe, false).Result;
                if (result.Succeeded)
                    return Ok(new { status = "OK" });
                else
                    return StatusCode(401, new { description = "Введенные данные неверны" });
            }
            else
                return StatusCode(400, new { Errors = ModelState.Errors() });
        }

        [HttpDelete]
        [Authorize]
        public IActionResult LogOff()
        {
            loginManager.SignOutAsync().Wait();
            return Ok(new { status = "OK" });
        }

    }
}
