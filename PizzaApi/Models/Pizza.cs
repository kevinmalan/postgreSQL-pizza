namespace PizzaApi.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public List<PizzaTopping> Toppings { get; set; } = new();
    }
}