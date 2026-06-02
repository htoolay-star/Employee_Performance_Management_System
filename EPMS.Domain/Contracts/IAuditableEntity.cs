namespace EPMS.Domain.Contracts
{
    public interface IAuditableEntity
    {
        Guid PublicId { get; }
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
        long? CreatedBy { get; set; }
        long? UpdatedBy { get; set; }
    }
}
