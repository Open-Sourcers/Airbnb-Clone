using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<Tuple<int, int>> CountReviewsAdnSumStars(string propertyId);
    }
}
