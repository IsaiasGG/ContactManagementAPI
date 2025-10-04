using IF.ContactManagement.Domain.Common;

namespace IF.ContactManagement.Domain.Entities
{
    public class RoleModulePermission : BaseAuditableEntity
    {
        public string RoleId { get; set; } = null!;
        public string SystemModuleId { get; set; } = null!;
        public string PermissionTypeId { get; set; } = null!;

        public virtual Role? Role { get; set; }
        public virtual SystemModule? SystemModule { get; set; }
        public virtual PermissionType? PermissionType { get; set; }
    }
}
