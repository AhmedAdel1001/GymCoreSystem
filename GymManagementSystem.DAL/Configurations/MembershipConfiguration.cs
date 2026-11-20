using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("Memberships");

            builder.Property(m => m.StartDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.Member)
                   .WithMany(m => m.Memberships)
                   .HasForeignKey(m => m.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Plan)
                   .WithMany(p => p.Memberships)
                   .HasForeignKey(m => m.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

