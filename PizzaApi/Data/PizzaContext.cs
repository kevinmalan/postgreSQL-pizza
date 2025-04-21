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

                // ValueComparer for change-tracking of List<PizzaTopping>
                //var comparer = new ValueComparer<List<PizzaTopping>>(
                //    (a, b) => a.SequenceEqual(b, new PizzaToppingEquality()),
                //    list => list.Aggregate(
                //        0,
                //        (hash, item) => HashCode.Combine(hash, item.Id, item.Quantity)
                //    ),
                //    list => list.ToList()
                //);

                //prop.Metadata.SetValueComparer(comparer);
            });
        }
    }

    // Equality comparer for PizzaTopping
    public class PizzaToppingEquality : IEqualityComparer<PizzaTopping>
    {
        public bool Equals(PizzaTopping x, PizzaTopping y)
            => x.Id == y.Id && x.Quantity == y.Quantity;

        public int GetHashCode(PizzaTopping obj)
            => HashCode.Combine(obj.Id, obj.Quantity);
    }
}