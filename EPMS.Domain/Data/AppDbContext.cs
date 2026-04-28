using EPMS.Domain.Entities.App;
using EPMS.Domain.Entities.Audit;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EPMS.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // --- App Schema ---
        public DbSet<Notification> Notifications => Set<Notification>();

        // --- Audit Schema ---
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        // --- Auth Schema ---
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();

        // --- HR Schema ---
        public DbSet<Level> Levels => Set<Level>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<PositionPermission> PositionPermissions => Set<PositionPermission>();
        public DbSet<RatingScale> RatingScales => Set<RatingScale>();

        // --- Employee Info Schema ---
        public DbSet<EmployeeProfile> EmployeeProfiles => Set<EmployeeProfile>();
        public DbSet<EmployeeEmployment> EmployeeEmployments => Set<EmployeeEmployment>();
        public DbSet<EmployeePayrollInfo> EmployeePayrollInfos => Set<EmployeePayrollInfo>();

        // Shared Schema
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();

        // Performance Schema
        public DbSet<KPIMaster> KPIMasters => Set<KPIMaster>();
        public DbSet<PositionKPI> PositionKPIs => Set<PositionKPI>();
        public DbSet<KPIWeightPriority> KPIWeightPriorities => Set<KPIWeightPriority>();
        public DbSet<AppraisalCycle> AppraisalCycles => Set<AppraisalCycle>();
        public DbSet<Appraisal> Appraisals => Set<Appraisal>();
        public DbSet<AppraisalDetail> AppraisalDetails => Set<AppraisalDetail>();
        public DbSet<QuestionRatingScale> QuestionRatingScales => Set<QuestionRatingScale>();
        public DbSet<FormTemplate> FormTemplates => Set<FormTemplate>();
        public DbSet<FormQuestion> FormQuestions => Set<FormQuestion>();
        public DbSet<EvaluationResponse> EvaluationResponses => Set<EvaluationResponse>();
        public DbSet<ContinuousFeedback> ContinuousFeedbacks => Set<ContinuousFeedback>();
        public DbSet<OneOnOneMeeting> OneOnOneMeetings => Set<OneOnOneMeeting>();
        public DbSet<PIP> PIPs => Set<PIP>();
        public DbSet<PIPObjective> PIPObjectives => Set<PIPObjective>();
        public DbSet<AppraisalRecommendation> AppraisalRecommendations => Set<AppraisalRecommendation>();
        public DbSet<PositionFormTemplate> PositionFormTemplates => Set<PositionFormTemplate>();
        public DbSet<PositionPIPTemplate> PositionPIPTemplates => Set<PositionPIPTemplate>();

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateTimeOffset>().HaveColumnType("datetimeoffset");
            configurationBuilder.Properties<DateOnly>().HaveColumnType("date");
            configurationBuilder.Properties<decimal>().HaveColumnType("decimal(18,2)"); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Appraisal>(entity =>
            {
                entity.HasOne(a => a.Employee)
                      .WithMany()
                      .HasForeignKey(a => a.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Appraiser)
                      .WithMany()
                      .HasForeignKey(a => a.AppraiserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}