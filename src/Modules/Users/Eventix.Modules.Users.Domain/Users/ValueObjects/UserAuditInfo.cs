using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Users.Domain.Users.ValueObjects
{
    public record UserAuditInfo : ValueObject
    {
        public UserAuditInfo(bool isDeleted, DateTime createdAtUtc, DateTime? deletedAtUtc = null)
        {
            IsDeleted = isDeleted;
            CreatedAtUtc = createdAtUtc;
            DeletedAtUtc = deletedAtUtc;
            Validate();
        }

        private UserAuditInfo()
        { }

        public bool IsDeleted { get; }
        public DateTime CreatedAtUtc { get; }
        public DateTime? DeletedAtUtc { get; }

        public static implicit operator UserAuditInfo((bool isDeleted, DateTime createdAtUtc, DateTime? deletedAtUtc) auditInfo)
            => new(auditInfo.isDeleted, auditInfo.createdAtUtc, auditInfo.deletedAtUtc);

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}