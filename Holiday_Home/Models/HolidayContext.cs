using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Holiday_Home.Models
{
    public class HolidayContext : DbContext
    {
        public HolidayContext(DbContextOptions<HolidayContext> options)
            : base(options) { }

        public DbSet<HolidayOwner> HolidayOwners { get; set; }
        public DbSet<HolidayHome> HolidayHomes { get; set; }

    }
}


