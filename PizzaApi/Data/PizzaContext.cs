using Microsoft.EntityFrameworkCore;
using PizzaApi.Models;
using System.Text.Json;

namespace PizzaApi.Data
{
    public class PizzaContext(DbContextOptions<PizzaContext> options) : DbContext(options)
    {
        public DbSet<Topping> Toppings { get; set; } = null!;
        public DbSet<Pizza> Pizzas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map Toppings table
            modelBuilder.Entity<Topping>(entity =>
            {
                entity.ToTable("toppings");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id");
                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired();
            });

            // Map Pizzas table with JSONB column for toppings
            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.ToTable("pizzas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id");
                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired();

                // JSONB mapping for the toppings list
                var toppingsProp = entity.Property(e => e.Toppings)
                    .HasColumnName("toppings")
                    .HasColumnType("jsonb")
                    .HasConversion(
                        // CLR -> JSON
                        list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null)!,
                        // JSON -> CLR
                        json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null)!
                    );
            });
        }
    }
}