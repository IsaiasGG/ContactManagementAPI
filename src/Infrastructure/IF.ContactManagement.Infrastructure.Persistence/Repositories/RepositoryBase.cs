using IF.ContactManagement.Application.DTO.Pagination;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IF.ContactManagement.Infrastructure.Persistence.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseAuditableEntity
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity == null ? false : true;
        }

        public async Task<bool> ExistsAsync(params object?[]? keyValuyes)
        {
            var entity = await _context.Set<T>().FindAsync(keyValuyes);
            return entity == null ? false : true;
        }

        public async Task<T> AddAsync(T entity, string username)
        {
            var now = DateTime.UtcNow;
            entity.CreatedAt = now;
            entity.CreatedBy = username;

            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Added;

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities.ToList();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(bool includeErased = false)
        {
            if (!includeErased)
            {
                return await _context.Set<T>().Where(x => x.DeletedAt == null).ToListAsync();
            }

            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                     string includeString = null,
                                                     bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            //            if (!string.IsNullOrEmpty(includeString))
            if (!string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                     List<Expression<Func<T, object>>> includes = null,
                                                     bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(bool includeErased = false,
                                               Expression<Func<T, bool>> predicate = null,
                                               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                               List<Expression<Func<T, object>>> includes = null,
                                               bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!includeErased)
            {
                query = query.Where(x => x.DeletedAt == null);
            }

            if (disableTracking)
                query = query.AsNoTracking();

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).FirstOrDefaultAsync();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(params object?[]? keyValuyes)
        {
            return await _context.Set<T>().FindAsync(keyValuyes);
        }

        public async Task<PagedResult<T>> GetPagedAsync(int page,
                                                        int limit,
                                                        bool includeErased = false,
                                                        Expression<Func<T, bool>> predicate = null,
                                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                        List<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!includeErased)
            {
                query = query.Where(x => x.DeletedAt == null);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var include in includes ?? new List<Expression<Func<T, object>>>())
            {
                query = query.Include(include);
            }

            var totalItems = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query.Skip((page - 1) * limit)
                                   .Take(limit)
                                   .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                Meta = new GenericPaginationResponse
                {
                    TotalItems = totalItems,
                    ItemCount = items.Count,
                    ItemsPerPage = limit,
                    TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                    CurrentPage = page
                }
            };
        }

        public async Task<T> SoftDeleteAsync(T entity, string deletedBy = "System")
        {
            var now = DateTime.UtcNow;
            entity.DeletedAt = now;
            entity.DeletedBy = deletedBy;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var now = DateTime.UtcNow;
            _context.Set<T>().Attach(entity);
            entity.UpdatedAt = now;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public void AddEntity(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddEntityRange(List<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void UpdateEntity(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteEntity(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity == null)
            {
                return false;
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FundContact>> GetByFundIdAsync(int fundId)
        {
            return await _context.FundContacts
                .Where(fc => fc.FundId == fundId && fc.DeletedAt == null && fc.Contact.DeletedAt == null)
                .Include(fc => fc.Contact)
                .ToListAsync();
        }

        public async Task<bool> IsAssignedToAnyFundAsync(int contactId, CancellationToken cancellationToken = default)
        {
            return await _context.FundContacts.AnyAsync(fc => fc.ContactId == contactId && fc.DeletedAt == null, cancellationToken);
        }
    }
}
