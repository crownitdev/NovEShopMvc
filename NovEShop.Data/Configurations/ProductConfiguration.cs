using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovEShop.Data.Models;

namespace NovEShop.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.OriginalPrice).IsRequired();
            builder.Property(p => p.Stock).IsRequired().HasDefaultValue<int>(0);
            builder.Property(p => p.ViewCount).IsRequired().HasDefaultValue<int>(0);
            builder.Property(p => p.IsActive).HasDefaultValue(true);
        }
    }
}
