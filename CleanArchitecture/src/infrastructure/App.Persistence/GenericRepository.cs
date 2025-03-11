using App.Application.Contracts.Persistence;
using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistence
{
    public class GenericRepository<T, TId>(AppDbContext context) : IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct
    {
        protected AppDbContext Context = context;

        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<bool> AnyAsync(TId id) => await _dbSet.AnyAsync(x => x.Id!.Equals(id));
        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking();

        public ValueTask<T?> GetByIdAsync(int id) => _dbSet.FindAsync(id);
        public void Update(T entity) => _dbSet.Update(entity);

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

        public async Task<List<T>> GetAllAsync() =>await _dbSet.ToListAsync();

        public async Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize) =>await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}
