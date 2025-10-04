using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Interfaces;

namespace IF.ContactManagement.Domain.Entities
{
    public class PermissionType : BaseAuditableEntity, ISharedEntity
    {
        public string PermissionTypeId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }

        public virtual ICollection<RoleModulePermission> RoleModulePermissions { get; set; } = new List<RoleModulePermission>();
    }
}
