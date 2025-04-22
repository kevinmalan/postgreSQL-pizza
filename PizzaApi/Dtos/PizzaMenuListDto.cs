namespace PizzaApi.Dtos
{
    public class PizzaMenuListDto
    {
        public List<PizzaMenuDto> Pizzas { get; set; }
        public List<ToppingDto> AdditionalToppingOptions { get; set; }
    }
}