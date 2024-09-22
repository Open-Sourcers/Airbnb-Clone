using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Interfaces.Interface;
namespace Airbnb.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        #region Without Specification
        Task<IEnumerable<TEntity>>? GetAllAsync();
        Task<TEntity>? GetByIdAsync(TKey id);
        #endregion

        #region With Specification
        Task<IReadOnlyList<TEntity>>? GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        Task<TEntity>? GetEntityWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        #endregion
        Task<TEntity>? GetByNameAsync(string entity);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void UpdateRange(IEnumerable<TEntity> entities);
 
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
