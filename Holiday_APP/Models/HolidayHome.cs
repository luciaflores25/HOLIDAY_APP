using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holiday_Home.Models
{
    public class HolidayHome
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double RentalPrice { get; set; }
        public int HomeOwnerId { get; set; }
    }
}
