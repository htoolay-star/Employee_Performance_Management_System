using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Interface.Irepo.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
