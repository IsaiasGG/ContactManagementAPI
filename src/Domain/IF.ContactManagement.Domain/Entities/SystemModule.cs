using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Interfaces;

namespace IF.ContactManagement.Domain.Entities
{
    public class SystemModule : BaseAuditableEntity, ISharedEntity
    {
        public string SystemModuleId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }
        public string? ParentId { get; set; }
        public string? SystemModuleCategorieId { get; set; }

        public virtual SystemModuleCategory? SystemModuleCategory { get; set; }
        public virtual SystemModule? Parent { get; set; }
        public virtual ICollection<SystemModule> Children { get; set; } = new List<SystemModule>();
        public virtual ICollection<RoleModulePermission> RoleModulePermissions { get; set; } = new List<RoleModulePermission>();
    }
}
