using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovEShop.Data.Models;

namespace NovEShop.Data.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(o => o.Product)
                .WithMany(m => m.Carts)
                .HasForeignKey(fk => fk.ProductId);

            builder.HasOne(o => o.ApplicationUser)
                .WithMany(m => m.Carts)
                .HasForeignKey(fk => fk.UserId);
        }
    }
}
