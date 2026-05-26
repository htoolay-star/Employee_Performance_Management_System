using EPMS.Domain.Interface.Irepo.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace EPMS.Domain.Repository.Shared
{
    public class SharedModule(IServiceProvider serviceProvider) : ISharedModule
    {
        private ICategoryRepository? _categories;
        private IDocumentAttachmentRepository? _documentAttachments;

        public ICategoryRepository Categories =>
            _categories ??= serviceProvider.GetRequiredService<ICategoryRepository>();

        public IDocumentAttachmentRepository DocumentAttachments =>
            _documentAttachments ??= serviceProvider.GetRequiredService<IDocumentAttachmentRepository>();
    }
}
