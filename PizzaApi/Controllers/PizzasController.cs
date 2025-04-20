using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaApi.Data;
using PizzaApi.Models;

namespace PizzaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzasController : ControllerBase
    {
        private readonly PizzaContext _db;
        public PizzasController(PizzaContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Pizzas.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Pizza pizza)
        {
            _db.Pizzas.Add(pizza);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = pizza.Id }, pizza);
        }
    }
}
