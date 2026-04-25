using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Info;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Info
{
    public class InfoModule : IInfoModule
    {
        private readonly AppDbContext _context;
        private IEmployeeProfileRepository? _profiles;

        public InfoModule(AppDbContext context) => _context = context;

        public IEmployeeProfileRepository EmployeeProfiles => _profiles ??= new EmployeeProfileRepository(_context);
    }
}
