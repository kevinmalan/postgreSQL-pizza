namespace PizzaApi.Dtos
{
    public class PizzaDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public List<ToppingDto> Toppings { get; set; } = new();
    }
}