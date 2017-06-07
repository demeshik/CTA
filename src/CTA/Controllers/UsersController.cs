using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CTA.Context;
using CTA.Services;
using Microsoft.EntityFrameworkCore;
using CTA.Models;
using AutoMapper;
using CTA.DTO;
using CloudinaryDotNet;

namespace CTA.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly DBContext db;
        private readonly ICloudInterface cloud;
        public UsersController(DBContext _db, ICloudInterface _cloud)
        {
            db = _db;
            cloud = _cloud;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult GetLot(string id)
        {
            if (int.TryParse(id, out int _id))
            {
                db.Users.Load();
                User user = db.Users.Local.First(lt => lt.Id == _id);

                Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());

                var tupel = new Tuple<UserDTO, Cloudinary>(Mapper.Map<UserDTO>(user), cloud.Configuration());

                return View("User", tupel);
            }
            else
                return BadRequest();
        }
    }
}
