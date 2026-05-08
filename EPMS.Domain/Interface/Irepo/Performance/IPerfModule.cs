using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Performance
{
public interface IPerfModule
{
    IAppraisalRepository Appraisals { get; }
    IKPIWeightPriorityRepository KPIWeightPriorities { get; }
    IRatingScaleRepository RatingScales { get; }
    IAppraisalCycleRepository AppraisalCycles { get; }
    IKPIMasterRepository KPIMasters { get; }
    IPIPRepository PIPs { get; }
    IFormTemplateRepository FormTemplates { get; }
    IFormQuestionRepository FormQuestions { get; }
    IContinuousFeedbackRepository ContinuousFeedbacks { get; }
    IOneOnOneMeetingRepository OneOnOneMeetings { get; }
    IPositionKPIRepository PositionKPIs { get; }
    IQuestionRatingScaleRepository QuestionRatingScales { get; }
    IPositionPIPTemplateRepository PositionPIPTemplates { get; }
    IPositionFormTemplateRepository PositionFormTemplates { get; }
    IPIPObjectiveRepository PIPObjectives { get; }
}
}
