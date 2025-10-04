
namespace IF.ContactManagement.Application.DTO.User
{
    public class UserModulePermissionDTO
    {
        public string ModuleId { get; set; } = null!;
        public string ModuleName { get; set; } = null!;
        public string? ModuleUrl { get; set; }
        public string PermissionTypeId { get; set; } = null!;
        public string PermissionTypeName { get; set; } = null!;
    }
}
