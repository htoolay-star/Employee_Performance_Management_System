using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Configurations.Hr
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> entity)
        {
            entity.ToTable("Teams", "hr");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.PublicId).IsRequired();
            entity.HasIndex(e => e.PublicId).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.Code).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.Property(e => e.Code).HasMaxLength(20).IsRequired();

            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => new { e.DepartmentId, e.Name }).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasIndex(e => e.LeadTeamId).IsUnique();
            entity.Property(e => e.LeadTeamId);
            entity.HasOne(e => e.LeadTeam)
                  .WithMany()
                  .HasForeignKey(e => e.LeadTeamId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.IsActive).HasFilter("[IsActive] = 1");

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.Property(e => e.Version).IsRowVersion();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false).IsRequired();
            entity.Property(e => e.DeletedAt);

            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Teams)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
