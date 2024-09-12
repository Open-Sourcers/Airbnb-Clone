using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;

namespace Airbnb.Application.Services
{
    public class ReviewServices : IReviewService
    {

        private readonly IGenericRepository<Review, int> _genericRepository;
        private readonly IGenericRepository<Property, int> _genericPropertyRepository;
        public ReviewServices(IGenericRepository<Review, int> genericRepository, IGenericRepository<Property, int> genericPropertyRepository)
        {
            _genericRepository = genericRepository;
            _genericPropertyRepository = genericPropertyRepository;

        }
        public async Task AddReviewAsync(Review review)
        {
            await _genericRepository.AddAsync(review);
        }

        public async Task DeleteReviewAsync(int id)
        {

            var review = await _genericRepository.GetByIdAsync(id);
           if (review != null)
            {
                _genericRepository.Remove(review);
            }
            else
            {
                throw new KeyNotFoundException($"Review with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _genericRepository.GetAllAsync(); }
    
        public Task<Review> GetReviewAsync(int id)
        {
            var review= _genericRepository.GetByIdAsync(id);
            return review;
        }

        public async Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(int PropertyId)
        {
            var property = await _genericPropertyRepository.GetByIdAsync(PropertyId);
            return property.Reviews;
        }    
    }
}
