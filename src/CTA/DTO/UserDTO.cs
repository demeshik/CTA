using CTA.Models;
using System.Collections.Generic;

namespace CTA.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Lot> Lots { get; set; }
        public ICollection<Bid> Bids { get; set; }
    }
}
