﻿namespace PizzaApi.Dtos
{
    public class ToppingDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}