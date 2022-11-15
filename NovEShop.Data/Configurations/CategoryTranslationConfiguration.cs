using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovEShop.Data.Models;

namespace NovEShop.Data.Configurations
{
    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.ToTable("CategoryTranslations");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.SeoDescription).IsRequired().HasMaxLength(500);
            builder.Property(p => p.SeoAlias).IsRequired().HasMaxLength(200);
            builder.Property(p => p.SeoTitle).IsRequired().HasMaxLength(200);
            builder.Property(p => p.LanguageId).IsUnicode(false).IsRequired().HasMaxLength(5);

            builder.HasOne(o => o.Language)
                .WithMany(m => m.CategoryTranslations)
                .HasForeignKey(fk => fk.LanguageId);

            builder.HasOne(o => o.Category)
                .WithMany(m => m.CategoryTranslations)
                .HasForeignKey(fk => fk.CategoryId);
        }
    }
}
