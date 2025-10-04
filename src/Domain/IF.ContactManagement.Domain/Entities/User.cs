using IF.ContactManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace IF.ContactManagement.Domain.Entities
{
    public class User : IdentityUser<string>
    {
        public string? FullName { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public override string? Email { get; set; }
        public override string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public override string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IsOwner { get; set; }

        public virtual ICollection<UserPermissionsAssignment> UserPermissionsAssignments { get; set; } = new List<UserPermissionsAssignment>();
        public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();
    }
}
