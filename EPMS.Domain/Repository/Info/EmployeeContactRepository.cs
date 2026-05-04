using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeContactRepository : GenericRepository<EmployeeContact>, IEmployeeContactRepository
    {
        public EmployeeContactRepository(AppDbContext context) : base(context) { }
    }
}
