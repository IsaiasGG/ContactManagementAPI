using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Infrastructure.Persistence.Context;

namespace IF.ContactManagement.Infrastructure.Persistence.Repositories
{
    public class FundRepository : RepositoryBase<Fund>, IFundRepository
    {
        public FundRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
