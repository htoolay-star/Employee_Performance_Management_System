using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Auth Schema
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();

        // HR Schema
        public DbSet<Level> Levels => Set<Level>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<PositionPermission> PositionPermissions => Set<PositionPermission>();
        public DbSet<RatingScale> RatingScales => Set<RatingScale>();

        // Employee Info Schema
        public DbSet<EmployeeProfile> EmployeeProfiles => Set<EmployeeProfile>();
        public DbSet<EmployeeEmployment> EmployeeEmployments => Set<EmployeeEmployment>();
        public DbSet<EmployeePayrollInfo> EmployeePayrollInfos => Set<EmployeePayrollInfo>();

        // Shared Schema
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();

        // Performance Schema
        public DbSet<KPIMaster> KPIMasters => Set<KPIMaster>();
        public DbSet<PositionKPI> PositionKPIs => Set<PositionKPI>();
        public DbSet<KPIWeightPriority> PerformanceReviews => Set<KPIWeightPriority>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
