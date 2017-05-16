﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.Models
{
    public class User:IdentityUser<int>
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string CreditCard { get; set; }
    }
}
