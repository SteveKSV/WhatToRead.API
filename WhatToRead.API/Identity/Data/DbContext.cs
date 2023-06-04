using Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class JwtDbContext : IdentityDbContext<User>
    {
        public JwtDbContext(DbContextOptions<JwtDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seeding a  'Administrator' role to AspNetRoles table
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() },
                new IdentityRole { Id = "4d6c234e-4b2e-526g-86af-483e56fd8345", Name = "User", NormalizedName = "USER".ToUpper() }
                );

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<User>();

            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    FirstName = "MyFirstName",
                    LastName = "MyLastName",
                    UserName = "MyUserName",
                    Email = "user1@example.com",
                    NormalizedUserName = "MYUSER",
                    PasswordHash = hasher.HashPassword(null, "Pa$$w0rd")
                },
                new User
                {
                    Id = "9c445865-a24d-4233-a6c6-9443d048cdb9", // primary key
                    FirstName = "MyFirstName2",
                    LastName = "MyLastName2",
                    UserName = "MyUserName2",
                    Email = "user2@example.com",
                    NormalizedUserName = "MYUSER2",
                    PasswordHash = hasher.HashPassword(null, "Pa$$w0rd")
                }
            );

            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "4d6c234e-4b2e-526g-86af-483e56fd8345",
                    UserId = "9c445865-a24d-4233-a6c6-9443d048cdb9"
                }
            );
        }
    }
}
