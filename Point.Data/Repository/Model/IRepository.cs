using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Point.Data.Repository.Model
{
    public interface IRepository<T> : IDisposable where T : class
    {
        bool Exists(long id);
        Task<bool> ExistsAsync(long id);

        T Get(long id, bool trackEntity = true);
        Task<T> GetAsync(long id, bool trackEntity = true);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, bool trackEntities = true);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, bool trackEntities = true);
        IEnumerable<T> GetAll(bool trackEntities = true);
        Task<IEnumerable<T>> GetAllAsync(bool trackEntities = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        T Add(T entity);
        Task<T> AddAsync(T entity);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);
        Task UpdateAsync(T entity);
        void UpdateRange(IEnumerable<T> entities);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        void Remove(long id);
        Task RemoveAsync(long id);
        void Remove(T entity);
        Task RemoveAsync(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}