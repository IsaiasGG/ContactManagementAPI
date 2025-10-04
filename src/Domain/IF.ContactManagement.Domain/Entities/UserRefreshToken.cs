using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Interfaces;

namespace IF.ContactManagement.Domain.Entities
{
    public class UserRefreshToken : BaseAuditableEntity, ISharedEntity
    {
        public string UserRefreshTokenId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
