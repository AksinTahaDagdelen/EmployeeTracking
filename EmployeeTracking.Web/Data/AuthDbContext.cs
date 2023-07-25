using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var managerRoleId = "db9f64e3-2708-4a45-ab63-c59201d49a1f";
            var ceoRoleId = "0121fdd9-f253-4553-ad52-51acb0b11607";
            var employeeRoleId = "10d3d3f2-dfc5-43c3-adfa-8bb0e6b70078";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Manager",
                    NormalizedName ="Manager",
                    Id=managerRoleId,
                    ConcurrencyStamp = managerRoleId
                },
                    new IdentityRole
                {
                    Name = "Ceo",
                    NormalizedName ="Ceo",
                    Id=ceoRoleId,
                    ConcurrencyStamp = ceoRoleId
                },
                     new IdentityRole
                {
                    Name = "Employee",
                    NormalizedName ="Employee",
                    Id=employeeRoleId,
                    ConcurrencyStamp = employeeRoleId
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);


            var ceoId = "c8c28db1-9787-4642-b762-d27ca5ecf37a";
            var ceoUser = new IdentityUser
            {
                UserName = "ceo@employeetracking.com",
                Email = "ceo@employeetracking.com",
                NormalizedEmail = "ceo@employeetracking.com".ToUpper(),
                NormalizedUserName = "ceo@employeetracking.com".ToUpper(),
                Id = ceoId
            };

            //Password created for ceo 
            ceoUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(ceoUser, "Kerem.Aksin.Berkay55");

            builder.Entity<IdentityUser>().HasData(ceoUser);

            //Add All Roles To SuperAdmin

            var ceoRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId= managerRoleId,
                    UserId = ceoId
                },
                 new IdentityUserRole<string>
                {
                    RoleId= ceoRoleId,
                    UserId = ceoId
                },
                  new IdentityUserRole<string>
                {
                    RoleId= employeeRoleId,
                    UserId = ceoId
                },


            };
            builder.Entity<IdentityUserRole<string>>().HasData(ceoRoles);
        }
    }
}
