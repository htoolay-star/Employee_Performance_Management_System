using EPMS.Domain.Interface.Irepo.Performance;
using Microsoft.Extensions.DependencyInjection;

namespace EPMS.Domain.Repository.Performance
{
    public class PerfModule(IServiceProvider serviceProvider) : IPerfModule
    {
        private IAppraisalRepository? _perfAppraisalRepository;
        private IAppraisalRecommendationRepository? _perfAppraisalRecommendationRepository;
        private IEvaluationResponseRepository? _perfEvaluationResponseRepository;
        private IRatingScaleRepository? _perfRatingScaleRepository;
        private IKPIWeightPriorityRepository? _perfKPIWeightPriorityRepository;
        private IAppraisalCycleRepository? _perfAppraisalCycleRepository;
        private IKPIMasterRepository? _perfKPIMasterRepository;
        private IPIPRepository? _perfPIPRepository;
        private IFormTemplateRepository? _perfFormTemplateRepository;
        private IFormQuestionRepository? _perfFormQuestionRepository;
        private IContinuousFeedbackRepository? _perfContinuousFeedbackRepository;
        private IOneOnOneMeetingRepository? _perfOneOnOneMeetingRepository;
        private IEntityKPIRepository? _perfEntityKPIRepository;
        private IEntityKPIHistoryRepository? _perfEntityKPIHistoryRepository;
        private IEmployeeKPIRepository? _perfEmployeeKPIRepository;
        private IEmployeeKPIHistoryRepository? _perfEmployeeKPIHistoryRepository;
        private IQuestionRatingScaleRepository? _perfQuestionRatingScaleRepository;
        private IQuestionRatingScaleLevelRepository? _perfQuestionRatingScaleLevelRepository;
        private IPositionPIPTemplateRepository? _perfPositionPIPTemplateRepository;
        private IPositionFormTemplateRepository? _perfPositionFormTemplateRepository;
        private IPIPObjectiveRepository? _perfPIPObjectiveRepository;

        public IAppraisalRepository Appraisals =>
            _perfAppraisalRepository ??= serviceProvider.GetRequiredService<IAppraisalRepository>();

        public IAppraisalRecommendationRepository AppraisalRecommendations =>
            _perfAppraisalRecommendationRepository ??= serviceProvider.GetRequiredService<IAppraisalRecommendationRepository>();

        public IEvaluationResponseRepository EvaluationResponses =>
            _perfEvaluationResponseRepository ??= serviceProvider.GetRequiredService<IEvaluationResponseRepository>();

        public IRatingScaleRepository RatingScales =>
            _perfRatingScaleRepository ??= serviceProvider.GetRequiredService<IRatingScaleRepository>();

        public IKPIWeightPriorityRepository KPIWeightPriorities =>
            _perfKPIWeightPriorityRepository ??= serviceProvider.GetRequiredService<IKPIWeightPriorityRepository>();

        public IAppraisalCycleRepository AppraisalCycles =>
            _perfAppraisalCycleRepository ??= serviceProvider.GetRequiredService<IAppraisalCycleRepository>();

        public IKPIMasterRepository KPIMasters =>
            _perfKPIMasterRepository ??= serviceProvider.GetRequiredService<IKPIMasterRepository>();

        public IPIPRepository PIPs =>
            _perfPIPRepository ??= serviceProvider.GetRequiredService<IPIPRepository>();

        public IFormTemplateRepository FormTemplates =>
            _perfFormTemplateRepository ??= serviceProvider.GetRequiredService<IFormTemplateRepository>();

        public IFormQuestionRepository FormQuestions =>
            _perfFormQuestionRepository ??= serviceProvider.GetRequiredService<IFormQuestionRepository>();

        public IContinuousFeedbackRepository ContinuousFeedbacks =>
            _perfContinuousFeedbackRepository ??= serviceProvider.GetRequiredService<IContinuousFeedbackRepository>();

        public IOneOnOneMeetingRepository OneOnOneMeetings =>
            _perfOneOnOneMeetingRepository ??= serviceProvider.GetRequiredService<IOneOnOneMeetingRepository>();

        public IEntityKPIRepository EntityKPIs =>
            _perfEntityKPIRepository ??= serviceProvider.GetRequiredService<IEntityKPIRepository>();

        public IEntityKPIHistoryRepository EntityKPIHistories =>
            _perfEntityKPIHistoryRepository ??= serviceProvider.GetRequiredService<IEntityKPIHistoryRepository>();

        public IEmployeeKPIRepository EmployeeKPIs =>
            _perfEmployeeKPIRepository ??= serviceProvider.GetRequiredService<IEmployeeKPIRepository>();

        public IEmployeeKPIHistoryRepository EmployeeKPIHistories =>
            _perfEmployeeKPIHistoryRepository ??= serviceProvider.GetRequiredService<IEmployeeKPIHistoryRepository>();

        public IQuestionRatingScaleRepository QuestionRatingScales =>
            _perfQuestionRatingScaleRepository ??= serviceProvider.GetRequiredService<IQuestionRatingScaleRepository>();

        public IQuestionRatingScaleLevelRepository QuestionRatingScaleLevels =>
            _perfQuestionRatingScaleLevelRepository ??= serviceProvider.GetRequiredService<IQuestionRatingScaleLevelRepository>();

        public IPositionPIPTemplateRepository PositionPIPTemplates =>
            _perfPositionPIPTemplateRepository ??= serviceProvider.GetRequiredService<IPositionPIPTemplateRepository>();

        public IPositionFormTemplateRepository PositionFormTemplates =>
            _perfPositionFormTemplateRepository ??= serviceProvider.GetRequiredService<IPositionFormTemplateRepository>();

        public IPIPObjectiveRepository PIPObjectives =>
            _perfPIPObjectiveRepository ??= serviceProvider.GetRequiredService<IPIPObjectiveRepository>();
    }
}
