using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain;

namespace Airbnb.Domain.Interfaces.Interface
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; }
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }

        public int Take { get; set; }
        public int Skip {  get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
