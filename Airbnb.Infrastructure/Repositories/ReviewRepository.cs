
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Domain.Interfaces.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AirbnbDbContext _dbContext;

        public ReviewRepository(AirbnbDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tuple<int, int>> CountReviewsAdnSumStars(string propertyId)
        {
            var count=await _dbContext.Reviews.CountAsync(x=>x.PropertyId==propertyId);
            var summ = await _dbContext.Reviews.Where(x => x.PropertyId == propertyId).SumAsync(x => x.Stars);
            return  Tuple.Create(count, summ);
        }
    }
}
