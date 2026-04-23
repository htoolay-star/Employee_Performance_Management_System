using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.Performance
{
    public class KPIWeightPriorityConfiguration : IEntityTypeConfiguration<KPIWeightPriority>
    {
        public void Configure(EntityTypeBuilder<KPIWeightPriority> entity)
        {
            entity.ToTable("KPIWeightPriorities", "perf");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.HasIndex(e => e.LevelName).IsUnique();
            entity.Property(e => e.LevelName).HasMaxLength(50).IsRequired();

            entity.Property(e => e.MinWeight).IsRequired();
            entity.Property(e => e.MaxWeight).IsRequired();

            entity.Property(e => e.ColorCode).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
