using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Infrastructure.Persistence.Context;
using System.Collections;

namespace IF.ContactManagement.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable? repositories;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        private IUserRefreshTokenRepository userRefreshTokenRepository;
        public IUserRefreshTokenRepository UserRefreshTokenRepository => userRefreshTokenRepository ??= new UserRefreshTokenRepository(context);
        public IFundRepository FundRepository { get;}
        public IFundContactRepository FundContactRepository { get;}
        public IContactRepository ContactRepository { get;}

        public UnitOfWork(ApplicationDbContext context,       
                          IFundRepository fundRepository,
                          IFundContactRepository fundContactRepository,
                          IContactRepository contactRepository,
                          IMapper mapper)
        {
            this.context = context;
            FundRepository = fundRepository;
            FundContactRepository = fundContactRepository;
            ContactRepository = contactRepository;
            this.mapper = mapper;
        }

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IAsyncRepository<T> Repository<T>() where T : BaseAuditableEntity
        {
            if (repositories == null)
            {
                repositories = new Hashtable();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(IAsyncRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }

            return (IAsyncRepository<T>)repositories[type];
        }

        public void SetUser(string userId)
        {
            context.UserId = userId;
        }
    }
}
