using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Witcher3IngredientsMVC.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var superAdminRoleId = "d1ef9859-5ef3-4fdb-8864-c1ef54e80c71";
            var roles = new List<IdentityRole>
            {
                new IdentityRole{
                Name="SuperAdmin",
                NormalizedName="SuperAdmin",
                Id=superAdminRoleId,
                ConcurrencyStamp=superAdminRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            //////////////////////////////
            var superAdminId = "092c2e00-c642-48d1-af85-0acfa6a97741";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");
            builder.Entity<IdentityUser>().HasData(superAdminUser);



            ///////////////////////////////////
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                 new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
