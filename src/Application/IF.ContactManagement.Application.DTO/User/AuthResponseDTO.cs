using IF.ContactManagement.Application.DTO.SystemModule;

namespace IF.ContactManagement.Application.DTO.User
{
    public class AuthResponseDTO
    {
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public List<string> UserRoles { get; set; }
        public List<UserModulePermissionDTO> ModulePermissions { get; set; } = new();
        public List<SystemModuleDTO> AccessibleModules { get; set; } = new();
    }
}
