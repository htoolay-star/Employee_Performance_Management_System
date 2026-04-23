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
    public class PositionFormTemplateConfiguration : IEntityTypeConfiguration<PositionFormTemplate>
    {
        public void Configure(EntityTypeBuilder<PositionFormTemplate> builder)
        {
            builder.ToTable("PositionFormTemplates", "perf");

            builder.HasKey(e => new { e.PositionId, e.FormTemplateId });

            builder.Property(e => e.IsMandatory).HasDefaultValue(true);

            builder.HasOne(e => e.Position)
                   .WithMany(p => p.PositionFormTemplates)
                   .HasForeignKey(e => e.PositionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.FormTemplate)
                   .WithMany()
                   .HasForeignKey(e => e.FormTemplateId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();
        }
    }
}
