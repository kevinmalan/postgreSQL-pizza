using Microsoft.EntityFrameworkCore;
using PizzaApi.Data;
using PizzaApi.Dtos;
using PizzaApi.Models;
using PizzaApi.Services.Interfaces;

namespace PizzaApi.Services
{
    public class ManuService(PizzaContext db) : IMenuService
    {
        public async Task<PizzaMenuListDto> GetPizzaMenuAsync()
        {
            var menuResponse = new PizzaMenuListDto();
            var pizzasResponse = new List<PizzaMenuDto>();
            var dbPizzas = await db.Pizzas.ToListAsync();
            var dbToppings = await db.Toppings.ToListAsync();
            foreach (var pizza in dbPizzas)
            {
                var dto = new PizzaMenuDto
                {
                    Id = pizza.Id,
                    Name = pizza.Name,
                    Price = pizza.Price,
                    Toppings = []
                };
                foreach (PizzaTopping topping in pizza.Toppings)
                {
                    var toppingResult = dbToppings.Find(x => x.Id == topping.Id);
                    if (toppingResult is null) continue;
                    dto.Toppings.Add(new PizzaToppingMenuDto
                    {
                        Id = toppingResult.Id,
                        Name = toppingResult.Name
                    });
                }

                pizzasResponse.Add(dto);
            }

            menuResponse.Pizzas = pizzasResponse;
            menuResponse.AdditionalToppingOptions = dbToppings.Select(x => new ToppingDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price
            }).ToList();

            return menuResponse;
        }
    }
}