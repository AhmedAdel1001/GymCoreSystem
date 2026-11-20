using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions", t =>
            {
                t.HasCheckConstraint("CK_Session_Capacity", "Capacity BETWEEN 1 AND 25");
                t.HasCheckConstraint("CK_Session_EndDateAfterStartDate", "EndDate > StartDate");
            });

            builder.Property(s => s.Description)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.HasOne(s => s.Trainer)
                   .WithMany(t => t.Sessions)
                   .HasForeignKey(s => s.TrainerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Category)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(s => s.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

