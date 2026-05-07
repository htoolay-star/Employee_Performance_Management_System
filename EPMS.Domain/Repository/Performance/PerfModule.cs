using AutoMapper;
using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Interface.Irepo.Performance;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Performance
{
public class PerfModule(IServiceProvider serviceProvider) : IPerfModule
{
    private IAppraisalRepository? _perfAppraisalRepository;
    private IRatingScaleRepository? _perfRatingScaleRepository;
    private IKPIWeightPriorityRepository? _perfKPIWeightPriorityRepository;
    private IAppraisalCycleRepository? _perfAppraisalCycleRepository;
    private IKPIMasterRepository? _perfKPIMasterRepository;
    private IPIPRepository? _perfPIPRepository;
    private IFormTemplateRepository? _perfFormTemplateRepository;
    private IContinuousFeedbackRepository? _perfContinuousFeedbackRepository;
    private IOneOnOneMeetingRepository? _perfOneOnOneMeetingRepository;
    private IPositionKPIRepository? _perfPositionKPIRepository;
    private IQuestionRatingScaleRepository? _perfQuestionRatingScaleRepository;
    private IPositionPIPTemplateRepository? _perfPositionPIPTemplateRepository;

    public IAppraisalRepository Appraisals =>
        _perfAppraisalRepository ??= serviceProvider.GetRequiredService<IAppraisalRepository>();

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

    public IContinuousFeedbackRepository ContinuousFeedbacks =>
        _perfContinuousFeedbackRepository ??= serviceProvider.GetRequiredService<IContinuousFeedbackRepository>();

    public IOneOnOneMeetingRepository OneOnOneMeetings =>
        _perfOneOnOneMeetingRepository ??= serviceProvider.GetRequiredService<IOneOnOneMeetingRepository>();

    public IPositionKPIRepository PositionKPIs =>
        _perfPositionKPIRepository ??= serviceProvider.GetRequiredService<IPositionKPIRepository>();

    public IQuestionRatingScaleRepository QuestionRatingScales =>
        _perfQuestionRatingScaleRepository ??= serviceProvider.GetRequiredService<IQuestionRatingScaleRepository>();

    public IPositionPIPTemplateRepository PositionPIPTemplates =>
        _perfPositionPIPTemplateRepository ??= serviceProvider.GetRequiredService<IPositionPIPTemplateRepository>();
}
}
