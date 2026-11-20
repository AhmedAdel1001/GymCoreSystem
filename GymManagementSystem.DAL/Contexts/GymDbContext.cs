using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagementSystem.DAL.Contexts
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        public DbSet<GymUser> GymUsers { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Trainer> Trainers { get; set; } = null!;
        public DbSet<HealthRecord> HealthRecords { get; set; } = null!;
        public DbSet<Plan> Plans { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<Membership> Memberships { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

