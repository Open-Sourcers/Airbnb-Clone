using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AirbnbDbContext _context;
        private readonly Dictionary<string, object> _repositories = new();

        public UnitOfWork(AirbnbDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(typeName))
            {
                var repositoryInstance = new GenericRepository<TEntity, TKey>(_context);
                _repositories[typeName] = repositoryInstance;
            }

            return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
