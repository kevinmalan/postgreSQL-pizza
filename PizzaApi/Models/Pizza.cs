namespace PizzaApi.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // We'll store an array of topping names in a JSONB column
        public List<string> Toppings { get; set; } = new();
    }
}
