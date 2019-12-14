using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holiday_Home.Models;

namespace Holiday_Home.Controllers
{
    [Route("api/holidayhome")]
    [ApiController]
    public class HolidayHomeController : ControllerBase
    {
        private readonly HolidayContext _context;

        public HolidayHomeController(HolidayContext context)
        {
            _context = context;

            //Since it uses InMemoryDatabase, there is no data from the start, therefore data is created
            if (_context.HolidayHomes.Count() == 0)
            {
                _context.HolidayHomes.AddRange(
                    new HolidayHome { Address = "Cortijo casas viejas, Granada", RentalPrice = 780, HomeOwnerId = 3 },
                    new HolidayHome { Address = "El valle dorado, Huelva", RentalPrice = 320, HomeOwnerId = 4 },
                    new HolidayHome { Address = "Caserío tradicional, Málaga", RentalPrice = 650, HomeOwnerId = 3 },
                    new HolidayHome { Address = "Cortijo Quintillo, Granada", RentalPrice = 425, HomeOwnerId = 3 },
                    new HolidayHome { Address = "Casa rural villa Anna, Málaga", RentalPrice = 545, HomeOwnerId = 1 });
                _context.SaveChanges();
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<HolidayHome>>> GetHolidayHomes()
        {
            return await _context.HolidayHomes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HolidayHome>> GetHolidayHome(int id)
        {
            var holidayHome = await _context.HolidayHomes.FindAsync(id);

            if (holidayHome == null)
            {
                return NotFound();
            }

            return holidayHome;
        }

        [HttpPost]
        public async Task<ActionResult<HolidayHome>> PostHolidayHome(HolidayHome hHome)
        {
            //Checks if the Home owner doesn't exits
            if (!await _context.HolidayOwners.AnyAsync(h => h.Id == hHome.HomeOwnerId))
            {
                return Content("Holiday Home Owner does not exist");
            }

            _context.HolidayHomes.Add(hHome);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHolidayHome), new { id = hHome.Id }, hHome);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHolidayHome(int id, HolidayHome hHome)
        {
            if (id != hHome.Id)
            {
                return BadRequest();
            }

            _context.Entry(hHome).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHolidayHome(int id)
        {
            var holidayHome = await _context.HolidayHomes.FindAsync(id);

            if (holidayHome == null)
            {
                return NotFound();
            }

            _context.HolidayHomes.Remove(holidayHome);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}