using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovEShop.Data.Models;
using NovEShop.Data.Models.Commons;
using System;
using System.Linq;

namespace NovEShop.Data.Helpers
{
    public static class Seed
    {
        public static void SeedSampleData(ModelBuilder builder)
        {
            SeedAppConfigs(builder);
            SeedLanguages(builder);
            SeedCategories(builder);
            SeedProducts(builder);
        }

        private static void SeedAppConfigs(ModelBuilder builder)
        {
            builder.Entity<AppConfig>().HasData(
               new AppConfig() { Key = "HomeTitle", Value = "NovEShop" },
               new AppConfig() { Key = "HomeKeyword", Value = "Keyword of NovEShop" },
               new AppConfig() { Key = "HomeDescription", Value = "Ecommerce Store Website" }
               );
        }

        private static void SeedLanguages(ModelBuilder builder)
        {
            builder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en-US", Name = "English", IsDefault = false }
            );
        }

        private static void SeedCategories(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    IsActive = true
                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     IsActive = true
                 }
                );

            builder.Entity<CategoryTranslation>().HasData(
                    new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Áo nam", LanguageId = "vi-VN", SeoAlias = "ao-nam", SeoDescription = "Sản phẩm áo thời trang nam", SeoTitle = "Sản phẩm áo thời trang nam" },
                    new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men Shirt", LanguageId = "en-US", SeoAlias = "men-shirt", SeoDescription = "The shirt products for men", SeoTitle = "The shirt products for men" },
                    new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Áo nữ", LanguageId = "vi-VN", SeoAlias = "ao-nu", SeoDescription = "Sản phẩm áo thời trang nữ", SeoTitle = "Sản phẩm áo thời trang women" },
                    new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Women Shirt", LanguageId = "en-US", SeoAlias = "women-shirt", SeoDescription = "The shirt products for women", SeoTitle = "The shirt products for women" }
            );
        }

        private static void SeedProducts(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
               new Product()
               {
                   Id = 1,
                   CreatedAt = DateTime.Now,
                   OriginalPrice = 100000,
                   Price = 200000,
                   Stock = 0,
                   ViewCount = 0,
               });

            builder.Entity<ProductTranslation>().HasData(
                 new ProductTranslation()
                 {
                     Id = 1,
                     ProductId = 1,
                     Name = "Áo sơ mi nam trắng Việt Tiến",
                     LanguageId = "vi-VN",
                     SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                     SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                     SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                     Details = "Áo sơ mi nam trắng Việt Tiến",
                     Description = "Áo sơ mi nam trắng Việt Tiến"
                 },
                new ProductTranslation()
                {
                    Id = 2,
                    ProductId = 1,
                    Name = "Viet Tien Men T-Shirt",
                    LanguageId = "en-US",
                    SeoAlias = "viet-tien-men-t-shirt",
                    SeoDescription = "Viet Tien Men T-Shirt",
                    SeoTitle = "Viet Tien Men T-Shirt",
                    Details = "Viet Tien Men T-Shirt",
                    Description = "Viet Tien Men T-Shirt"
                }
            );
        }

        public static void SeedData(NovEShopDbContext db)
        {
            SeedRoles(db);
            SeedUsers(db);
        }

        private static void SeedRoles(NovEShopDbContext db)
        {
            var roles = new[]
            {
                new ApplicationRole() { Name = "Administrator", NormalizedName = "ADMINISTRATOR"},
                new ApplicationRole() { Name = "Moderator", NormalizedName = "MODERATOR" },
                new ApplicationRole() { Name = "Customer", NormalizedName = "CUSTOMER" }
            };

            bool needSaveChanges = false;

            foreach (var role in roles)
            {
                if (!db.Roles.Any(r => r.Name == role.Name))
                {
                    db.Roles.Add(role);
                    needSaveChanges = true;
                }
            }

            if (needSaveChanges)
            {
                db.SaveChanges();
            }
        }

        private static void SeedUsers(NovEShopDbContext db)
        {
            if (!db.Users.Any(u => u.UserName == "admin"))
            {
                var role = db.Roles.Where(r => r.Name == "Administrator").FirstOrDefault();
                CreateUser(db, "admin", "nvquang.19it5@vku.udn.vn", "@Admin123", "System", "Admin", role.Id);
            }

            if (!db.Users.Any(u => u.UserName == "moderator"))
            {
                var role = db.Roles.Where(r => r.Name == "Moderator").FirstOrDefault();
                CreateUser(db, "moderator", "ndngo.19it5@vku.udn.vn", "@Moderator123", "System", "Moderator", role.Id);
            }
        }


        private static void CreateUser(NovEShopDbContext db, string username, string email, string pwd, string firstName, string lastName, int roleId)
        {
            var user = new ApplicationUser()
            {
                UserName = username,
                FirstName = firstName,
                LastName = lastName,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            var hashed = hasher.HashPassword(user, pwd);
            user.PasswordHash = hashed;
            db.Users.Add(user);
            db.SaveChanges();
            db.UserRoles.Add(new IdentityUserRole<int> { UserId = user.Id, RoleId = roleId });
            db.SaveChanges();
        }
    }
}
