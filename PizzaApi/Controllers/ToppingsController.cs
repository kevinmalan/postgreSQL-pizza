using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaApi.Data;
using PizzaApi.Models;

namespace PizzaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToppingsController : ControllerBase
    {
        private readonly PizzaContext _db;
        public ToppingsController(PizzaContext db) => _db = db;

        /// <summary>
        /// Retrieves all toppings.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var toppings = await _db.Toppings.ToListAsync();
            return Ok(toppings);
        }

        /// <summary>
        /// Retrieves a single topping by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var topping = await _db.Toppings.FindAsync(id);
            if (topping is null)
                return NotFound();
            return Ok(topping);
        }

        /// <summary>
        /// Creates a new topping.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Topping topping)
        {
            _db.Toppings.Add(topping);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = topping.Id }, topping);
        }
    }
}
