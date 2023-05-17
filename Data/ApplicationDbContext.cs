using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinema.Models;

namespace Cinema.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cinema.Models.Movie>? Movie { get; set; }
        public DbSet<Cinema.Models.Seance>? Seance { get; set; }
        public DbSet<Cinema.Models.Reservation>? Reservation { get; set; }
    }
}