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
            foreach (var pizza in pizzas)
            {
                var dto = new PizzaDto
                {
                    Name = pizza.Name,
                    Price = pizza.Price,
                    Toppings = new List<ToppingDto>()
                };
                foreach (PizzaTopping topping in pizza.Toppings)
                {
                    var toppingResult = await db.Toppings.FindAsync(topping.Id);
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