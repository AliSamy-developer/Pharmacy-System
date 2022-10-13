using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Pharmacy.Services
{
    public interface IGenericServices<T> where T : class
    {
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<T> GetAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> criteria,
            Func<IQueryable<T>, IIncludableQueryable<T,
                object>> include = null);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    }
}
