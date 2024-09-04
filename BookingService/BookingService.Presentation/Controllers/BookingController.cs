using BookingService.Application.Models;
using BookingService.Infrastructure.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ScyllaDbContext _context;
        public BookingController(ScyllaDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            await _context.SaveBookingAsync(booking);
            return Ok(booking);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(string id)
        {
            var booking = await _context.GetBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

    }
}
