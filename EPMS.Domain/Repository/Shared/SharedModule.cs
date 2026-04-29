using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Shared
{
    public class SharedModule : ISharedModule
    {
        private readonly AppDbContext _context;

        private ICategoryRepository? _categories;
        private ITagRepository? _tags;

        public SharedModule(AppDbContext context) => _context = context;

        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public ITagRepository Tags => _tags ??= new TagRepository(_context);
    }
}
