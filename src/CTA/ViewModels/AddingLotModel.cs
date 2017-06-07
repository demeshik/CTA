using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.ViewModels
{
    public class AddingLotModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string MainImage { get; set; }
        [Required]
        public string Images { get; set; }

        [Required]
        public DateTime ExpiredDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public int MinBid { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
