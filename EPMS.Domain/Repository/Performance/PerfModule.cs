using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Performance
{
    public class PerfModule : IPerfModule
    {
        private readonly AppDbContext _context;

        private IAppraisalRepository? _perfAppraisalRepository;

        public PerfModule(AppDbContext context) => _context = context;

        public IAppraisalRepository Appraisals => _perfAppraisalRepository ??= new AppraisalRepository(_context);
    }
}
