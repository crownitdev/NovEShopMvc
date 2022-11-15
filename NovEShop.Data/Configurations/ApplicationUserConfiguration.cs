using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovEShop.Data.Models.Commons;

namespace NovEShop.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUsers");

            builder.Property(p => p.FirstName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Dob).HasMaxLength(200).IsRequired();
        }
    }
}
