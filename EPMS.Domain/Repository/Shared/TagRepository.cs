using EPMS.Domain.Data;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Shared
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
