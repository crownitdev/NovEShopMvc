using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovEShop.Data.Models;

namespace NovEShop.Data.Configurations
{
    public class ProductCategoriesConfiguration : IEntityTypeConfiguration<ProductCategories>
    {
        public void Configure(EntityTypeBuilder<ProductCategories> builder)
        {
            builder.ToTable("ProductCategories");

            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder.HasOne(o => o.Product)
                .WithMany(m => m.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(o => o.Category)
                .WithMany(m => m.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
