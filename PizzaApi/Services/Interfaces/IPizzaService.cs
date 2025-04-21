using PizzaApi.Dtos;

namespace PizzaApi.Services.Interfaces
{
    public interface IPizzaService
    {
        Task<List<PizzaDto>> GetPizzasAsync();
    }
}