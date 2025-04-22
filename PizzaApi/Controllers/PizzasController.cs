using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaApi.Data;
using PizzaApi.Models;
using PizzaApi.Services.Interfaces;

namespace PizzaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzasController(PizzaContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pizzas = await db.Pizzas.ToListAsync();
            return Ok(pizzas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pizza pizza)
        {
            db.Pizzas.Add(pizza);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = pizza.Id }, pizza);
        }
    }
}