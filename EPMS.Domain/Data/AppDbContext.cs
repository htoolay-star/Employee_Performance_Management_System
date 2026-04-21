using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
