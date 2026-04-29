using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;

namespace EPMS.Domain.Repository.Hr
{
    public class RatingScaleRepository : GenericRepository<RatingScale>, IRatingScaleRepository
    {
        public RatingScaleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
