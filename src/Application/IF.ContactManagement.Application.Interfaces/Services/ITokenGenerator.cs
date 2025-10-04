using IF.ContactManagement.Domain.Entities;

namespace IF.ContactManagement.Application.Interfaces.Services
{
    public interface ITokenGenerator
    {
        public string GenerateJWTToken(User user, IList<string> roles);
        public string GenerateRefreshToken();

    }
}
