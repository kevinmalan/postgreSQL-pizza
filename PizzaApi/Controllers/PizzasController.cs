using Microsoft.AspNetCore.Mvc;
using PizzaApi.Data;
using PizzaApi.Models;
using PizzaApi.Services.Interfaces;

namespace PizzaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzasController(PizzaContext db, IPizzaService pizzaService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await pizzaService.GetPizzasAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Pizza pizza)
        {
            db.Pizzas.Add(pizza);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = pizza.Id }, pizza);
        }
    }
}