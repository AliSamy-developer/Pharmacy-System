using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pharmacy.Data;
using System.Linq.Expressions;

namespace Pharmacy.Services
{
    public class GenericServices<T> : IGenericServices<T> where T : class
    {
        private readonly ApplicationDbContext _context;
    public GenericServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(T entity)
        {
             await _context.Set<T>().AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var query = _context.Set<T>().AsQueryable<T>();
            if (include != null)
            {
                query = include(query);
            }
            return await query.SingleOrDefaultAsync(criteria);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var query = _context.Set<T>().AsQueryable<T>();
            query = query.Where(criteria);
            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            var result = await _context.SaveChangesAsync();
            if(result > 0)
                return true;
            return false;
        }
    }
}
