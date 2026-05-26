namespace EPMS.Domain.Interface.Irepo.Shared
{
    public interface ISharedModule
    {
        ICategoryRepository Categories { get; }
        IDocumentAttachmentRepository DocumentAttachments { get; }
    }
}
