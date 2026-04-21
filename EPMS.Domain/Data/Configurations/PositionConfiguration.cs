using EPMS.Domain.Entities.Hr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> entity)
        {
            entity.ToTable("Positions", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.HasIndex(e => e.Title).IsUnique();
            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.Property(e => e.CreatedAt).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");

            entity.HasOne(e => e.Level)
                  .WithMany()
                  .HasForeignKey(e => e.LevelId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
