﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.Models
{
    public class Lot
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MainImage { get; set; }
        public string Images { get; set; }

        public DateTime ExpiredDate { get; set; }

        [DataType(DataType.Currency)]
        public int MinBid { get; set; }

        [DataType(DataType.Currency)]
        public int CurrBid { get; set; }


        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
        public Lot()
        {
            Bids = new List<Bid>();
        }
    }
}
