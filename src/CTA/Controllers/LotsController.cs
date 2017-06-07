using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CTA.Context;
using CTA.Models;
using Microsoft.EntityFrameworkCore;
using CTA.ViewModels;
using CTA.Services;
using CloudinaryDotNet;

namespace CTA.Controllers
{
    [Route("[controller]")]
    public class LotsController : Controller
    {
        private readonly DBContext db;
        private readonly ICloudInterface cloud;
        public LotsController(DBContext _db, ICloudInterface _cloud)
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
                db.Lots.Load();
                Lot lot = db.Lots.Local.First(lt => lt.Id == _id);
                LotModel vLot = new LotModel()
                {
                    Id = lot.Id,
                    Title = lot.Title,
                    Description = lot.Description,
                    MainImage = lot.MainImage,
                    Images = lot.Images,
                    CurrBud = lot.CurrBid,
                    UserId = lot.UserId.ToString(),
                    ExpiredDate = lot.ExpiredDate,
                    Bids = lot.Bids
                };
                var tuple = new Tuple<LotModel, Cloudinary>(vLot, cloud.Configuration());
                return View("Lot",tuple);
            }
            else
                return BadRequest();
        }
    }
}
