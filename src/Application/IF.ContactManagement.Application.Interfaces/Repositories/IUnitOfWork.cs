using IF.ContactManagement.Domain.Common;

namespace IF.ContactManagement.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRefreshTokenRepository UserRefreshTokenRepository { get; }
        IFundRepository FundRepository { get; }
        IFundContactRepository FundContactRepository { get; }
        IContactRepository ContactRepository { get; }
        IAsyncRepository<T> Repository<T>() where T : BaseAuditableEntity;
        Task<int> CommitAsync();
        void SetUser(string userId);
    }
}
