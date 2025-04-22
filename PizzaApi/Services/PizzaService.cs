using Microsoft.EntityFrameworkCore;
using PizzaApi.Data;
using PizzaApi.Dtos;
using PizzaApi.Models;
using PizzaApi.Services.Interfaces;

namespace PizzaApi.Services
{
    public class PizzaService(PizzaContext db) : IPizzaService
    {
        public async Task<List<PizzaDto>> GetPizzasAsync()
        {
            var responseDto = new List<PizzaDto>();
            var pizzas = await db.Pizzas.ToListAsync();
            var toppings = await db.Toppings.ToListAsync();
            foreach (var pizza in pizzas)
            {
                var dto = new PizzaDto
                {
                    Name = pizza.Name,
                    Price = pizza.Price,
                    Toppings = []
                };
                foreach (PizzaTopping topping in pizza.Toppings)
                {
                    var toppingResult = toppings.Find(x => x.Id == topping.Id);
                    if (toppingResult is null) continue;
                    dto.Toppings.Add(new ToppingDto
                    {
                        Name = toppingResult.Name
                    });
                }

                responseDto.Add(dto);
            }

            return responseDto;
        }
    }
}