using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovEShop.Data.Configurations;
using NovEShop.Data.Helpers;
using NovEShop.Data.Models;
using NovEShop.Data.Models.Commons;

namespace NovEShop.Data
{
    public class NovEShopDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        int>
    {
        public NovEShopDbContext(DbContextOptions<NovEShopDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration<AppConfig>(new AppConfigConfiguration());
            builder.ApplyConfiguration<Cart>(new CartConfiguration());
            builder.ApplyConfiguration<Category>(new CategoryConfiguration());
            builder.ApplyConfiguration<CategoryTranslation>(new CategoryTranslationConfiguration());
            builder.ApplyConfiguration<Contact>(new ContactConfiguration());
            builder.ApplyConfiguration<Language>(new LanguageConfiguration());
            builder.ApplyConfiguration<Order>(new OrderConfiguration());
            builder.ApplyConfiguration<OrderDetail>(new OrderDetailConfiguration());
            builder.ApplyConfiguration<Product>(new ProductConfiguration());
            builder.ApplyConfiguration<ProductTranslation>(new ProductTranslationConfiguration());
            builder.ApplyConfiguration<ProductCategories>(new ProductCategoriesConfiguration());
            builder.ApplyConfiguration<Promotion>(new PromotionConfiguration());
            builder.ApplyConfiguration<Transaction>(new TransactionConfiguration());
            builder.ApplyConfiguration<ProductImage>(new ProductImageConfiguration());

            #region Configuration for Identity
            builder.ApplyConfiguration<ApplicationUser>(new ApplicationUserConfiguration());
            builder.ApplyConfiguration<ApplicationRole>(new ApplicationRoleConfiguration());

            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens").HasKey(x => x.UserId);
            #endregion

            #region Seed Data
            Seed.SeedSampleData(builder);
            #endregion
        }

        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<ProductCategories> ProductCategories { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
