using CTA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.Context
{
    public class DBContext:IdentityDbContext<User,Role,string>
    {
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {

        }
    }
}
