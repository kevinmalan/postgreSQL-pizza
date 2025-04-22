namespace PizzaApi.Dtos
{
    public class PizzaMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public List<PizzaToppingMenuDto> Toppings { get; set; } = new();
    }
}