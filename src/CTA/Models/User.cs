using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.Models
{
    public class User:IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CreditCard { get; set; }
        public ICollection<Lot> Lots { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public User()
        {
            Lots = new List<Lot>();
            Bids = new List<Bid>(); 
        }
    }
}
