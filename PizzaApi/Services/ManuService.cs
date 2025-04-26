using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PizzaApi.Data;
using PizzaApi.Dtos;
using PizzaApi.Models;
using PizzaApi.Services.Interfaces;
using System.Text.Json;

namespace PizzaApi.Services
{
    public class ManuService(PizzaContext db, IDistributedCache cache) : IMenuService
    {
        private const string _cacheKey = "PizzaMenu";

        public async Task<PizzaMenuListDto> GetPizzaMenuAsync()
        {
            var cached = await cache.GetStringAsync(_cacheKey);
            if (!string.IsNullOrWhiteSpace(cached))
            {
                return JsonSerializer.Deserialize<PizzaMenuListDto>(cached)!;
            }

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

            // Cache result
            var serialized = JsonSerializer.Serialize(menuResponse);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await cache.SetStringAsync(_cacheKey, serialized, options);

            return menuResponse;
        }
    }
}