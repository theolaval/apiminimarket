using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Domain.Models;

namespace MiniMarket.DAL.Database.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            // Properties configuration
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Price)
                .IsRequired();

            builder.Property(p => p.Discount)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            // Constraints
            builder.HasKey(p => p.Id);

            builder.HasData(new Product
            {
                Id = 1,
                Name = "iPhone 16",
                Price = 1299,
                Discount = 10,
                Stock = 50,
                Description = "This is a sample product description."
            });
        }
    }
}
