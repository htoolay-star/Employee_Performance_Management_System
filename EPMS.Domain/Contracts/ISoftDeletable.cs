namespace EPMS.Domain.Contracts
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTimeOffset? DeletedAt { get; set; }
        long? DeletedBy { get; set; }
    }
}
