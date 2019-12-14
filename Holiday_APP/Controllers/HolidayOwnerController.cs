using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holiday_Home.Models;

namespace Holiday_Home.Controllers
{
    [Route("api/holidayowner")]
    [ApiController]
    public class HolidayOwnerController : ControllerBase
    {
        private readonly HolidayContext _context;

        public HolidayOwnerController(HolidayContext context)
        {
            _context = context;

            if (_context.HolidayOwners.Count() == 0)
            {
                _context.HolidayOwners.AddRange(
                    new HolidayOwner { Name = "Anna", Id = 1 },
                    new HolidayOwner { Name = "Tiffany", Id = 2 },
                    new HolidayOwner { Name = "Tom", Id = 3 },
                    new HolidayOwner { Name = "Richard", Id = 4 },
                    new HolidayOwner { Name = "Daniel", Id = 5 },
                    new HolidayOwner { Name = "Peter", Id = 6 });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HolidayOwner>>> GetHolidayOwners()
        {
            return await _context.HolidayOwners.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HolidayOwner>> GetHolidayOwner(int id)
        {
            var holidayOwner = await _context.HolidayOwners.FindAsync(id);

            if (holidayOwner == null)
            {
                return NotFound();
            }

            return holidayOwner;
        }

        // /api/holidayowner/getHomes/1/0
        [HttpGet("{id}")]
        [Route("getHomes/{id}/{page}")]
        public async Task<ActionResult<IEnumerable<HolidayHome>>> GetHolidayOwnerHomes(int id, int page)
        {
            var holidayHomes = await _context.HolidayHomes.
                Where(hHome => hHome.HomeOwnerId == id).
                ToListAsync();

            if (holidayHomes == null)
            {
                return NotFound();
            }

            //Page should only consist of 5 elements
            //Page starts at 0
            return holidayHomes.Skip(5 * page).Take(5).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<HolidayOwner>> PostHolidayOwner(HolidayOwner hOwner)
        {
            _context.HolidayOwners.Add(hOwner);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHolidayOwner), new { id = hOwner.Id }, hOwner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHolidayOwner(int id, HolidayOwner hOwner)
        {
            if (id != hOwner.Id)
            {
                return BadRequest();
            }

            _context.Entry(hOwner).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHolidayOwner(int id)
        {
            var holidayOwner = await _context.HolidayOwners.FindAsync(id);
            var holidayHomes = await _context.HolidayHomes.Where(hHome => hHome.HomeOwnerId == id).ToListAsync();

            if (holidayOwner == null)
            {
                return NotFound();
            }

            //Sets all HomeOwnerIds for Holiday Homes, which the Home Owner is connected to, to 0 
            foreach (var hHome in holidayHomes)
            {
                _context.Entry(hHome).Property(h => h.HomeOwnerId).CurrentValue = 0;
            }

            _context.HolidayOwners.Remove(holidayOwner);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}