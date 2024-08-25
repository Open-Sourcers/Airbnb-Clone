using Airbnb.Domain;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly AirbnbDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AirbnbDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
           => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
          => await _dbSet.AddRangeAsync(entities);

        public void Remove(TEntity entity)
           => _dbSet.Remove(entity);


        public async Task<IEnumerable<TEntity>>? GetAllAsync()
          => await _dbSet.ToListAsync();


        public async Task<TEntity>? GetByIdAsync(TKey id)
          => (await _dbSet.FindAsync(id))!;


        public void Update(TEntity entity)
          => _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities)
         => _dbSet.UpdateRange(entities);
    }
}
