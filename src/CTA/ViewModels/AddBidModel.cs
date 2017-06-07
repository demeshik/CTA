using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.ViewModels
{
    public class AddBidModel
    {
        [Required]
        public DateTime Time { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int Amount { get; set; }
        [Required]
        public int LotId { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
