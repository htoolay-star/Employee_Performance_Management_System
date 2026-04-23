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
    public class FormTemplateConfiguration : IEntityTypeConfiguration<FormTemplate>
    {
        public void Configure(EntityTypeBuilder<FormTemplate> builder)
        {
            builder.ToTable("FormTemplates", "perf");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.FormType).HasMaxLength(50).IsRequired();
            builder.Property(e => e.IsActive).HasDefaultValue(true);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();
            builder.Property(e => e.Version).IsRowVersion();

            builder.HasMany(e => e.Questions)
                   .WithOne(q => q.Template)
                   .HasForeignKey(q => q.TemplateId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
