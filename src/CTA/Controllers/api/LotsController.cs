using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CTA.Models;
using CTA.Context;
using CTA.Utils;
using CTA.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace CTA.Controllers.api
{
    [Route("api/[controller]")]
    public class LotsController : Controller
    {
        private readonly DBContext db;
        private readonly UserManager<User> userManager;

        public LotsController(DBContext _db, UserManager<User> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string page, [FromQuery] string count)
        {
            db.Lots.Load();
            Mapper.Initialize(cfg => cfg.CreateMap<Lot, LotModel>());
            List<LotModel> lots = new List<LotModel>();

            var pagList = Utils.Utils.GetPaginationItems(ref page, ref count, db.Lots.Local);

            foreach (var lot in pagList)
            {
                lots.Add(Mapper.Map<Lot, LotModel>(lot));
            }
            return Json(new { meta = new { totalCount = db.Lots.Local.Count, page = page, count = count }, data = lots });
        }

        [HttpGet("{id}")]
        public ActionResult GetLot(string id)
        {
            if (int.TryParse(id, out int _id))
            {
                db.Lots.Load();
                var lot = db.Lots.Local.First(l => l.Id == _id);
                if (lot != null)
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<Lot, LotModel>());
                    return Ok(Mapper.Map<Lot, LotModel>(lot));
                }
                else
                    return NotFound();
            }
            else
                return StatusCode(400);
        }

        [HttpPost]
        public ActionResult Post([FromBody]AddingLotModel _lot)
        {
            if (ModelState.IsValid)
            {
                Lot newlot = new Lot()
                {
                    Title = _lot.Title,
                    Description = _lot.Description,
                    MainImage = _lot.MainImage,
                    Images = _lot.Images,
                    MinBid = _lot.MinBid,
                    ExpiredDate = _lot.ExpiredDate,
                    CurrBid = _lot.MinBid
                };

                User user = userManager.FindByNameAsync(_lot.UserId).Result;
                newlot.User = user;
                db.Lots.Add(newlot);
                db.SaveChanges();

                return Created($"/api/lots/", _lot);
            }
            else
               return StatusCode(400, new { Errors = ModelState.Errors() });
        }

        [HttpGet("{id}/bids")]
        public ActionResult GetBids(string id, [FromQuery] string page, [FromQuery] string count)
        {

            Lot lot = null;
            if (int.TryParse(id, out int _id))
                lot = db.Lots.Include(c => c.Bids).FirstOrDefault(c => c.Id == _id);

            Mapper.Initialize(cfg => cfg.CreateMap<Bid, BidModel>());
            var bidModels = new List<BidModel>();
            var pagList = Utils.Utils.GetPaginationItems(ref page, ref count, lot.Bids);
            foreach (var bid in pagList)
            {
                bidModels.Add(Mapper.Map<Bid, BidModel>(bid));
            }

            return Json(new { meta = new { totalCount = lot.Bids.Count, page = page, count = count }, data = bidModels });
        }

        
        [HttpGet("{id}/bids/max")]
        public int GetMaxBid(string id)
        {
            db.Lots.Load();
            int max = 0;
            if (int.TryParse(id, out int _id))
            {
                max = db.Lots.Local.First(lot => lot.Id == _id).CurrBid;
            }
            else
                return -1;
            return max;
        }

        [HttpPost("{id}/bids")]
        public ActionResult PostBid([FromBody]AddBidModel _bid)
        {
            if (ModelState.IsValid)
            {
                db.Lots.Load();
                var currLot = db.Lots.Local.First(lot => lot.Id == _bid.LotId);
                if (_bid.Amount <= currLot.CurrBid || _bid.Amount < currLot.MinBid)
                {
                    var errorsDictionary = new Dictionary<string, string>() { { "SmallBid", "Your bid is smaller than current bid." } };
                    return StatusCode(400, new { Errors = errorsDictionary.AsEnumerable() });
                }
                else
                {
                    if (DateTime.Now > currLot.ExpiredDate)
                    {
                        var errorsDictionary = new Dictionary<string, string>() { { "Timeout", "Bids for this lot are no longer accepted" } };
                        return StatusCode(400, new { Errors = errorsDictionary.AsEnumerable() });
                    }
                    currLot.CurrBid = _bid.Amount;
                    User user = userManager.FindByNameAsync(_bid.UserName).Result;
                    Bid bid = new Bid()
                    {
                        Time = _bid.Time,
                        Amount = _bid.Amount,
                        UserId = user.Id,
                        LotId = _bid.LotId
                    };

                    db.Bids.Add(bid);
                    db.SaveChanges();

                    return Created($"/lots/{_bid.LotId}/bids", _bid);
                }
            }
            else
                return StatusCode(400, new { Errors = ModelState.Errors() });
        }
    }
}

