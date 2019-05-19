using Microsoft.EntityFrameworkCore;
using Point.Data.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Point.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private bool disposedValue = false;
        private readonly DbSet<T> _dbSet;

        protected DbContext _context;

        public long Id { get; set; }

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public bool Exists(long id)
        {
            var entity = _dbSet.Find(id);

            Detach(entity);

            return entity != null;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            var entity = await _dbSet.FindAsync(id);

            Detach(entity);

            return entity != null;
        }

        public T Get(long id, bool trackEntity = true)
        {
            var entity = _dbSet.Find(id);

            if (!trackEntity)
            {
                Detach(entity);
            }

            return entity;
        }

        public async Task<T> GetAsync(long id, bool trackEntity = true)
        {
            var entity = await _dbSet.FindAsync(id);

            if (!trackEntity)
            {
                Detach(entity);
            }

            return entity;
        }
        
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = IncludeMultiple(_dbSet.Where(predicate), includes).AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, bool trackEntities = true)
        {
            var query = _dbSet.Where(predicate);

            if (!trackEntities)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, bool trackEntities = true)
        {
            var query = _dbSet.Where(predicate);

            if (!trackEntities)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public IEnumerable<T> GetAll(bool trackEntities = true)
        {
            var query = _dbSet.AsQueryable();

            if (!trackEntities)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool trackEntities = true)
        {
            var query = _dbSet.AsQueryable();

            if (!trackEntities)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = IncludeMultiple(_dbSet.Where(predicate), includes).AsNoTracking();

            return await query.AsNoTracking().ToListAsync();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            
            SaveChanges();

            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);

            await SaveChangesAsync();

            return entity;
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);

            SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);

            await SaveChangesAsync();
        }

        public void Remove(long id)
        {
            var entity = Get(id);

            if (entity == null)
                return;

            Remove(entity);
        }

        public async Task RemoveAsync(long id)
        {
            var entity = await GetAsync(id);

            if (entity == null)
                return;

            await RemoveAsync(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);

            SaveChanges();
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);

            await SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);

            SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);

            await SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;

            SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Modified;
            }

            SaveChanges();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Modified;
            }

            await SaveChangesAsync();
        }

        protected void ExcludePropertiesFromUpdate(T entity, params string[] properties)
        {
            if (entity == null)
            {
                return;
            }

            ExcludePropertiesFromUpdate(Enumerable.Repeat(entity, 1), properties);
        }

        protected void ExcludePropertiesFromUpdate(IEnumerable<T> entityCollection, params string[] properties)
        {
            if (entityCollection == null || !entityCollection.Any(x => x != null))
            {
                return;
            }

            if (properties == null || !properties.Any(x => !String.IsNullOrEmpty(x)))
            {
                return;
            }

            foreach (var entity in entityCollection)
            {
                foreach (var property in properties)
                {
                    if (!_context.Entry(entity).CurrentValues.Properties.Select(x => x.Name).Any(x => x.Equals(property)))
                        continue;

                    _context.Entry(entity).Property(property).IsModified = false;
                }
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }

        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private void Detach(T entity)
        {
            if (entity == null)
                return;

            _context.Entry(entity).State = EntityState.Detached;
        }

        #region IDisposable implementation

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }

        private IQueryable<T> IncludeMultiple(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        {
            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }
        #endregion
    }
}
