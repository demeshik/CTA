using CTA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.ViewModels
{
    public class LotModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MainImage { get; set; }
        public int CurrBud { get; set; }
        public string UserId { get; set; }
        public string Images { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int MinBid { get; set; }

        public ICollection<Bid> Bids { get; set; }
        public LotModel()
        {
            Bids = new List<Bid>();
        }
    }
}
