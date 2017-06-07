using AutoMapper;
using CTA.Context;
using CTA.DTO;
using CTA.Models;
using CTA.Utils;
using CTA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CTA.Controllers.api
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly DBContext db;

        public UsersController(UserManager<User> _userManager, RoleManager<Role> _roleManager,DBContext _db)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            db = _db;
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] string page, [FromQuery] string count)
        {
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            var users = Mapper.Map<IQueryable<User>, IEnumerable<UserDTO>>(userManager.Users);

            var userModels = new List<UserDTO>();
            var pagList = Utils.Utils.GetPaginationItems(ref page, ref count, userManager.Users);
            foreach (var user in pagList)
            {
                userModels.Add(Mapper.Map<User, UserDTO>(user));

            }
            return Json(new { meta = new { totalCount = userManager.Users.Count(), page = page, count = count }, data = userModels });
        }

        [HttpGet("{key}")]
        public ActionResult GetUserById(string key)
        {
            Models.User _user = null;
            if (int.TryParse(key, out int _id)) 
                _user = userManager.FindByIdAsync(key).Result;
            else
                _user = userManager.FindByNameAsync(key).Result;
            if (_user != null)
            {
                Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
                return Ok(Mapper.Map<User, UserDTO>(_user));
            }
            else
                return NotFound();

        }

        [HttpGet("{key}/lots")]
        public ActionResult GetUserLots(string key, [FromQuery] string page, [FromQuery] string count)
        {
            User user = null;
            if (int.TryParse(key, out int _id))
                user = db.Users.Include(c => c.Lots).FirstOrDefault(c => c.Id == _id);
            else
                user = db.Users.Include(c => c.Lots).FirstOrDefault(c => c.UserName == key);

            Mapper.Initialize(cfg => cfg.CreateMap<Lot, LotModel>());
            var lotModels = new List<LotModel>();
            var pagList = Utils.Utils.GetPaginationItems(ref page, ref count, user.Lots);
            foreach(var lot in pagList)
            {
                lotModels.Add(Mapper.Map<Lot, LotModel>(lot));

            }

            return Json(new { meta = new { totalCount = user.Lots.Count, page = page, count = count }, data = lotModels });
        }

        [HttpGet("{key}/bids")]
        public ActionResult GetUserBids(string key, [FromQuery] string page, [FromQuery] string count)
        {
            User user = null;
            if (int.TryParse(key, out int _id))
                user = db.Users.Include(c => c.Bids).FirstOrDefault(c => c.Id == _id);
            else
                user = db.Users.Include(c => c.Bids).FirstOrDefault(c => c.UserName == key);

            Mapper.Initialize(cfg => cfg.CreateMap<Bid, BidModel>());
            var bidModels = new List<BidModel>();
            var pagList = Utils.Utils.GetPaginationItems(ref page, ref count, user.Bids);
            foreach (var bid in pagList)
            {
                bidModels.Add(Mapper.Map<Bid, BidModel>(bid));

            }

            return Json(new { meta = new { totalCount = user.Bids.Count, page = page, count = count }, data = bidModels });
        }

        [HttpPost]
        public ActionResult Register([FromBody]RegisterUserModel user)
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
                    Image = user.Image,
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
                            return StatusCode(400, new { Errors = roleResult.Errors });
                        }
                    }
                    userManager.AddToRoleAsync(_user, "User").Wait();
                    Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
                    return Created($"/api/users/{_user.UserName}", Mapper.Map<User, UserDTO>(_user));
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return StatusCode(400, new { Errors = ModelState.Errors() });
        }

        [HttpPut]
        [Authorize]
        public ActionResult Update(UpdateUserModel _user)
        {
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;
            user.Name = _user.Name;
            user.UserName = _user.UserName;
            user.City = _user.City;
            user.Country = _user.Country;
            user.CreditCard = _user.CreditCard;
            user.Email = _user.Email;
            user.PhoneNumber = _user.PhoneNumber;
            user.Surname = _user.Surname;

            IdentityResult result = userManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                return Ok(new { status = "Updated" });
            }
            else
                return StatusCode(500, new { status = "Error", description = "User updated error" });
            
        }

        [HttpDelete("{id}")]
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
