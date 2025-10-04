using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Interfaces;

namespace IF.ContactManagement.Domain.Entities
{
    public class SystemModuleCategory : BaseAuditableEntity, ISharedEntity
    {
        public string SystemModuleCategoryId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }

        public virtual ICollection<SystemModule> SystemModules { get; set; } = new List<SystemModule>();
    }
}
