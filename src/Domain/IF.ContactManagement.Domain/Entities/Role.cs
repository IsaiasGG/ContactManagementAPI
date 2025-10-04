using IF.ContactManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IF.ContactManagement.Domain.Entities
{
    public class Role : IdentityRole<string>, ISharedEntity
    {
        public string? Description { get; set; }
        public string? Icon { get; set; }

        public virtual ICollection<UserPermissionsAssignment> UserPermissionsAssignments { get; set; } = new List<UserPermissionsAssignment>();
        public virtual ICollection<RoleModulePermission> RoleModulePermissions { get; set; } = new List<RoleModulePermission>();
    }
}
