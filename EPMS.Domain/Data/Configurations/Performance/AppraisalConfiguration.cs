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
    public class AppraisalConfiguration : IEntityTypeConfiguration<Appraisal>
    {
        public void Configure(EntityTypeBuilder<Appraisal> entity)
        {
            entity.ToTable("Appraisals", "perf");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.EmployeeId, e.CycleId }).IsUnique();

            entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
            entity.Property(e => e.TotalScore).HasColumnType("decimal(5,2)");
            entity.Property(e => e.RatingLabel).HasMaxLength(50);

            entity.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Cycle).WithMany().HasForeignKey(e => e.CycleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Appraiser).WithMany().HasForeignKey(e => e.AppraiserId).OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(e => e.Version).IsRowVersion();
        }
    }
}
