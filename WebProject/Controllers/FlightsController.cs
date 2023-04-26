using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Database;
using WebProject.WebModels;

namespace WebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly user05_1Context _context;
        private int input;

        public FlightsController(user05_1Context context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpPost("ListFlights")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
          if (_context.Flights == null)
          {
              return NotFound();
          }
            return await _context.Flights.Include(s => s.Airplane).Include(s => s.FlightCompany).Include(s => s.AirplaneClassFlights).ToListAsync();
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
          if (_context.Flights == null)
          {
              return NotFound();
          }
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight;
        }

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("put")]
        public async Task<IActionResult> PutFlight([FromBody] Flight flight)
        {
            input = flight.FlightsId;

            var origin = _context.Flights.Find(input);

            _context.Entry(origin).CurrentValues.SetValues(flight);

            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(input))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("save")]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
          if (_context.Flights == null)
          {
              return Problem("Entity set 'AviaSalesContext.Flights'  is null.");
          }
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.FlightsId }, flight);
        }

        // DELETE: api/Flights/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteFlight([FromBody] int id)
        {
            if (_context.Flights == null)
            {
                return NotFound();
            }
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightExists(int id)
        {
            return (_context.Flights?.Any(e => e.FlightsId == id)).GetValueOrDefault();
        }
    }
}
