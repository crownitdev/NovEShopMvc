using Microsoft.AspNetCore.Identity;
using NovEShop.Data.Models.Commons;
using System;
using System.Linq;

namespace NovEShop.Data.Helpers
{
    public static class Seed
    {
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
