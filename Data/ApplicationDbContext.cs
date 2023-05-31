using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace Cinema.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cinema.Models.Movie>? Movie { get; set; }
        public DbSet<Cinema.Models.News>? News { get; set; }
        public DbSet<Cinema.Models.Seance>? Seance { get; set; }
        public DbSet<Cinema.Models.Reservation>? Reservation { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            string ADMIN_ID = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            string ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

            //seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            //create user
            var appUser = new ApplicationUser
            {
                Id = ADMIN_ID,
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                UserName = "admin@gmail.com",
                Lastname="Nowak",
                Firstname="Adam",
             NormalizedUserName = "ADMIN@GMAIL.COM"
            };

            //set user password
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "mypassword_ ?");

            //seed user
            builder.Entity<ApplicationUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
            base.OnModelCreating(builder);
        }
    }
}