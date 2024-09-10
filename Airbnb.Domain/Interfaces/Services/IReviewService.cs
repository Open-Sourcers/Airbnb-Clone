using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;

namespace Airbnb.Domain.Interfaces.Services
{
    public interface IReviewService
    {
        Task<Review> GetReviewAsync(int id);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(int productId);
        Task AddReviewAsync(Review review);
        Task DeleteReviewAsync(int id);

    }
}
