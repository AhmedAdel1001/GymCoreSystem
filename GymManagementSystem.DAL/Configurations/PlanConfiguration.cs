using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GymManagementSystem.DAL.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("Plans", t =>
            {
                t.HasCheckConstraint("CK_Plan_DurationDays", "DurationDays BETWEEN 1 AND 365");
            });

            builder.Property(p => p.Name)
                   .HasColumnType("varchar(50)")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasColumnType("varchar(200)")
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(p => p.Price)
                   .HasPrecision(10, 2)
                   .IsRequired();

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            builder.HasData(
                new Plan
                {
                    Id = 1,
                    Name = "Basic",
                    Description = "Standard access to gym facilities and locker room",
                    DurationDays = 30,
                    Price = 500.00m,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Plan
                {
                    Id = 2,
                    Name = "Standard",
                    Description = "Access to gym plus 2 group classes per week",
                    DurationDays = 90,
                    Price = 1350.00m,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Plan
                {
                    Id = 3,
                    Name = "Premium",
                    Description = "Unlimited access to gym, classes, and sauna",
                    DurationDays = 180,
                    Price = 2500.00m,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Plan
                {
                    Id = 4,
                    Name = "Annual VIP",
                    Description = "Full year unlimited access with personal training session discounts",
                    DurationDays = 365,
                    Price = 4500.00m,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

