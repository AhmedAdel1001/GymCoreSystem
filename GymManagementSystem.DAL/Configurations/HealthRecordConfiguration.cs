using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations
{
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("HealthRecords");

            builder.Property(h => h.Height)
                   .HasPrecision(5, 2)
                   .IsRequired();

            builder.Property(h => h.Weight)
                   .HasPrecision(5, 2)
                   .IsRequired();

            builder.Property(h => h.BloodType)
                   .HasColumnType("varchar(5)")
                   .HasMaxLength(5)
                   .IsRequired();

            builder.Property(h => h.Note)
                   .HasColumnType("varchar(500)")
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(h => h.LastUpdate)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(h => h.MemberId)
                   .IsUnique();
        }
    }
}

