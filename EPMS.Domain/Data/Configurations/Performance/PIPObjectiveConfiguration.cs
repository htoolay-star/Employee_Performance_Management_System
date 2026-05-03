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
    public class PIPObjectiveConfiguration : IEntityTypeConfiguration<PIPObjective>
    {
        public void Configure(EntityTypeBuilder<PIPObjective> builder)
        {
            builder.ToTable("PIPObjectives", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.SuccessCriteria).IsRequired();
            builder.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("In-Progress");
            builder.Property(e => e.ManagerComment).HasMaxLength(500);

            builder.HasOne(e => e.PIP)
                   .WithMany(p => p.Objectives)
                   .HasForeignKey(e => e.PIPId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);
        }
    }
}
