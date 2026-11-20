using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(m => m.JoinDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(m => m.Photo)
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.HasOne(m => m.HealthRecord)
                   .WithOne(h => h.Member)
                   .HasForeignKey<HealthRecord>(h => h.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

