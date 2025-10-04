using IF.ContactManagement.Domain.Common;

namespace IF.ContactManagement.Domain.Entities
{
    public class UserPermissionsAssignment : BaseAuditableEntity
    {
        public string UserId { get; set; } = null!;
        public string RoleId { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;

    }
}
