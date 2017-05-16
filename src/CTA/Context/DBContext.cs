using CTA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.Context
{
    public class DBContext:IdentityDbContext<User,Role,int>
    {
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {

        }
    }
}
