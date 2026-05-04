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
    public class AppraisalDetailConfiguration : IEntityTypeConfiguration<AppraisalDetail>
    {
        public void Configure(EntityTypeBuilder<AppraisalDetail> entity)
        {
            entity.ToTable("AppraisalDetails", "perf");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.KPIName).HasMaxLength(250).IsRequired();
            entity.Property(e => e.KPIId);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.TargetValue).HasMaxLength(100);
            entity.Property(e => e.ActualValue).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.Property(e => e.Weightage).HasColumnType("decimal(5,2)").IsRequired();
            entity.Property(e => e.Score).HasColumnType("decimal(5,2)").IsRequired();
            entity.Property(e => e.WeightedScore).HasColumnType("decimal(5,2)").IsRequired();

            entity.HasOne(e => e.Appraisal)
                  .WithMany(a => a.Details)
                  .HasForeignKey(e => e.AppraisalId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Question)
                  .WithMany()
                  .HasForeignKey(e => e.QuestionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);
        }
    }
}
