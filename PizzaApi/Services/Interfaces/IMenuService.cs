using PizzaApi.Dtos;

namespace PizzaApi.Services.Interfaces
{
    public interface IMenuService
    {
        Task<PizzaMenuListDto> GetPizzaMenuAsync();
    }
}