using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Hr
{
    public class HRModule : IHRModule
    {
        private readonly AppDbContext _context;

        private IDepartmentRepository? _departments;
        private ITeamRepository? _teams;
        private IRatingScaleRepository? _ratingScales;

        public HRModule(AppDbContext context) => _context = context;

        public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context);
        public ITeamRepository Teams => _teams ??= new TeamRepository(_context);
        public IRatingScaleRepository RatingScales => _ratingScales ??= new RatingScaleRepository(_context);
    }
}
