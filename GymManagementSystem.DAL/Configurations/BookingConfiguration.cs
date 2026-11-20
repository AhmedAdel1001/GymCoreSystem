using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.Property(b => b.BookingDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(b => b.IsAttended)
                   .HasDefaultValue(false);

            builder.HasOne(b => b.Member)
                   .WithMany(m => m.Bookings)
                   .HasForeignKey(b => b.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Session)
                   .WithMany(s => s.Bookings)
                   .HasForeignKey(b => b.SessionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

