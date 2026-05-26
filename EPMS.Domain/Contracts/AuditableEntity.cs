namespace EPMS.Domain.Contracts
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        public Guid PublicId { get; init; } = Guid.NewGuid();

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
