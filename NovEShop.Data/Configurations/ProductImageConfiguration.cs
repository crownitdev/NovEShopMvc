using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovEShop.Data.Models;
using System;

namespace NovEShop.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(p => p.ImagePath).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Caption).HasMaxLength(200);

            builder.HasOne(o => o.Product)
                .WithMany(m => m.ProductImages)
                .HasForeignKey(fk => fk.ProductId);
        }
    }
}
