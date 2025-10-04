using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Infrastructure.Persistence.Context;

namespace IF.ContactManagement.Infrastructure.Persistence.Repositories
{
    public class UserRefreshTokenRepository : RepositoryBase<UserRefreshToken>, IUserRefreshTokenRepository
    {
        public UserRefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
