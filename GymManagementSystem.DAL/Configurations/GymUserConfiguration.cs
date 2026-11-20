using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations
{
    public class GymUserConfiguration : IEntityTypeConfiguration<GymUser>
    {
        public void Configure(EntityTypeBuilder<GymUser> builder)
        {
            builder.ToTable("GymUsers");

            builder.HasDiscriminator<string>("UserType")
                   .HasValue<Member>("Member")
                   .HasValue<Trainer>("Trainer");

            builder.Property(x => x.Name)
                   .HasColumnType("varchar(50)")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasColumnType("varchar(100)")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.Phone)
                   .HasColumnType("varchar(11)")
                   .HasMaxLength(11)
                   .IsRequired();

            builder.HasIndex(x => x.Phone).IsUnique();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_GymUser_Email", "Email LIKE '_%@_%._%'");
                t.HasCheckConstraint("CK_GymUser_Phone", "Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%'");
            });

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.BuildingNumber)
                       .HasColumnName("BuildingNumber");

                address.Property(a => a.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar(30)")
                       .HasMaxLength(30);

                address.Property(a => a.City)
                       .HasColumnName("City")
                       .HasColumnType("varchar(30)")
                       .HasMaxLength(30);
            });
        }
    }
}

