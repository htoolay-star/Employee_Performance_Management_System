using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Entities.Audit;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EPMS.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // --- App Schema ---
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

        // --- Audit Schema ---
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        // --- Auth Schema ---
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<PositionPermission> PositionPermissions => Set<PositionPermission>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();

        // --- HR Schema ---
        public DbSet<Level> Levels => Set<Level>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<RatingScale> RatingScales => Set<RatingScale>();

        // --- Employee Info Schema ---
        public DbSet<EmployeeContact> EmployeeContact => Set<EmployeeContact>();
        public DbSet<EmployeeEmployment> EmployeeEmployments => Set<EmployeeEmployment>();
        public DbSet<EmployeeEmploymentHistory> EmployeeEmploymentHistories => Set<EmployeeEmploymentHistory>();
        public DbSet<EmployeeFamilyInfo> EmployeeFamilyInfos => Set<EmployeeFamilyInfo>();
        public DbSet<EmployeePayrollInfo> EmployeePayrollInfos => Set<EmployeePayrollInfo>();
        public DbSet<EmployeeSalaryHistory> EmployeeSalaryHistories => Set<EmployeeSalaryHistory>();
        public DbSet<EmployeeProfile> EmployeeProfiles => Set<EmployeeProfile>();

        // Shared Schema
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<DocumentAttachment> DocumentAttachments => Set<DocumentAttachment>();

        // Performance Schema
        public DbSet<KPIMaster> KPIMasters => Set<KPIMaster>();
        public DbSet<PositionKPI> PositionKPIs => Set<PositionKPI>();
        public DbSet<PositionKPIHistory> PositionKPIHistories => Set<PositionKPIHistory>();
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
        public DbSet<AppraisalStatusHistory> AppraisalStatusHistories => Set<AppraisalStatusHistory>();
        public DbSet<PositionFormTemplate> PositionFormTemplates => Set<PositionFormTemplate>();
        public DbSet<PositionPIPTemplate> PositionPIPTemplates => Set<PositionPIPTemplate>();
        public DbSet<PIPStatusHistory> PIPStatusHistories => Set<PIPStatusHistory>();

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

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(
                        ConvertFilterExpression(entityType.ClrType)
                    );
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static LambdaExpression ConvertFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var falseConstant = Expression.Constant(false);
            var comparison = Expression.Equal(property, falseConstant);

            return Expression.Lambda(comparison, parameter);
        }
    }
}