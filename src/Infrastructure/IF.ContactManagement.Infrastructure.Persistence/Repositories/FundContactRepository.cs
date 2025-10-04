using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Infrastructure.Persistence.Context;

namespace IF.ContactManagement.Infrastructure.Persistence.Repositories
{
    public class FundContactRepository : RepositoryBase<FundContact>, IFundContactRepository
    {
        public FundContactRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
