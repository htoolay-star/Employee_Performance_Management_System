using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;

namespace EPMS.Domain.Repository.Performance;

public class QuestionRatingScaleLevelRepository : GenericRepository<QuestionRatingScaleLevel>, IQuestionRatingScaleLevelRepository
{
    public QuestionRatingScaleLevelRepository(AppDbContext context) : base(context) { }
}
