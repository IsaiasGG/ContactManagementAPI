using IF.ContactManagement.Application.DTO.Pagination;
using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Entities;
using System.Linq.Expressions;

namespace IF.ContactManagement.Application.Interfaces.Repositories
{
    public interface IAsyncRepository<T> where T : BaseAuditableEntity
    {
        Task<bool> ExistsAsync(string id);

        Task<bool> ExistsAsync(params object?[]? keyValuyes);
        Task<bool> IsAssignedToAnyFundAsync(int contactId, CancellationToken cancellationToken = default);
        Task<List<FundContact>> GetByFundIdAsync(int fundId);

        Task<IReadOnlyList<T>> GetAllAsync(bool includeErased = false);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includes = null,
                                        bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(bool includeErased = false,
                                        Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includes = null,
                                        bool disableTracking = true);

        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
                                       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                       List<Expression<Func<T, object>>> includes = null,
                                       bool disableTracking = true);

        Task<T?> GetByIdAsync(string id);

        Task<T?> GetByIdAsync(params object?[]? keyValuyes);

        Task<PagedResult<T>> GetPagedAsync(int page,
                                           int limit,
                                           bool includeErased = false,
                                           Expression<Func<T, bool>> predicate = null,
                                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                           List<Expression<Func<T, object>>> includes = null);

        Task<T> AddAsync(T entity, string username);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<T> SoftDeleteAsync(T entity, string deletedBy);


        void AddEntity(T entity);
        void AddEntityRange(List<T> entities);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
        Task<bool> DeleteByIdAsync(string id);
    }
}
