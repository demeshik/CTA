﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        [DataType(DataType.Currency)]
        public int Amount { get; set; }

        public int? LotId { get; set; }
        public virtual Lot Lot { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
