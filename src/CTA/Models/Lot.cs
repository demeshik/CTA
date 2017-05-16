using System;
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

        [DataType(DataType.Currency)]
        public int MaxBid { get; set; }

        public ICollection<Bid> Bids { get; set; }
        public Lot()
        {
            Bids = new List<Bid>();
        }
    }
}
