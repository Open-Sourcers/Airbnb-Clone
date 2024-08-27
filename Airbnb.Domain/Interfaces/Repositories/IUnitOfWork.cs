using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;

namespace Airbnb.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        Task<int> CompleteAsync();
    }
}
