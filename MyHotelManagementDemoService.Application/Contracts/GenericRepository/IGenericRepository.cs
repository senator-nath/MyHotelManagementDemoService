using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Contracts.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {

        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByColumnAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWhereAndIncludeAsync(
       Expression<Func<T, bool>> filter,
       Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<IEnumerable<T>> GetWhereAndIncludeQueryAsync(
   Expression<Func<T, bool>> predicate,
   Func<IQueryable<T>, IQueryable<T>> include);
    }
}
