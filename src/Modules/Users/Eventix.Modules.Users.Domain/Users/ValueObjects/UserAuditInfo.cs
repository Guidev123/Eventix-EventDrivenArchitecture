using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Users.Domain.Users.ValueObjects
{
    public record UserAuditInfo : ValueObject
    {
        public UserAuditInfo(DateTime createdAtUtc, bool isDeleted = false, DateTime? deletedAt = null)
        {
            IsDeleted = isDeleted;
            CreatedAtUtc = createdAtUtc;
            DeletedAtUtc = deletedAt;
            Validate();
        }

        private UserAuditInfo()
        { }

        public bool IsDeleted { get; }
        public DateTime CreatedAtUtc { get; }
        public DateTime? DeletedAtUtc { get; }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(CreatedAtUtc, UserErrors.CreatedAtMustBeNotEmpty.Description);

            if (IsDeleted)
            {
                AssertionConcern.EnsureNotNull(DeletedAtUtc!, UserErrors.DeletedAtUtcMustBeNotNullWhenUserIsDeleted.Description);
            }
            else
            {
                AssertionConcern.EnsureTrue(DeletedAtUtc is null, UserErrors.DeletedAtUtcMustBeNullWhenUserIsDeleted.Description);
            }
        }
    }
}