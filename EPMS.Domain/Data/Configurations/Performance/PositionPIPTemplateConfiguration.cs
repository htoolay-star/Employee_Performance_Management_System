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
    public class PositionPIPTemplateConfiguration : IEntityTypeConfiguration<PositionPIPTemplate>
    {
        public void Configure(EntityTypeBuilder<PositionPIPTemplate> builder)
        {
            builder.ToTable("PositionPIPTemplates", "perf");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasQueryFilter(e => !e.IsDeleted);

            builder.Property(e => e.PublicId).IsRequired();
            builder.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.SuccessCriteria).IsRequired();
            builder.Property(e => e.IsActive).HasDefaultValue(true);

            builder.HasOne(e => e.Position)
                   .WithMany(p => p.PositionPIPTemplates)
                   .HasForeignKey(e => e.PositionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.DeletedAt);
        }
    }
}
