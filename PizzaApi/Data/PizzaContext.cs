using Microsoft.EntityFrameworkCore;
using PizzaApi.Models;
using System.Text.Json;

namespace PizzaApi.Data
{
    public class PizzaContext : DbContext
    {
        public PizzaContext(DbContextOptions<PizzaContext> options)
            : base(options)
        {
        }

        public DbSet<Topping> Toppings { get; set; } = null!;
        public DbSet<Pizza> Pizzas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map toppings table
            modelBuilder.Entity<Topping>(entity =>
            {
                entity.ToTable("toppings");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.SizeGrams).HasColumnName("sizegrams");
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("numeric(10,2)");
                entity.Property(e => e.QtyStock).HasColumnName("qtystock");
            });

            // Map pizzas table
            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.ToTable("pizzas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("numeric(10,2)");

                // JSONB mapping for PizzaTopping list
                var prop = entity.Property(e => e.Toppings)
                    .HasColumnName("toppings")
                    .HasColumnType("jsonb")
                    .HasConversion(
                        list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null)!,  // CLR -> JSON
                        json => JsonSerializer.Deserialize<List<PizzaTopping>>(json, (JsonSerializerOptions?)null)! // JSON -> CLR
                    );
            });
        }
    }
}