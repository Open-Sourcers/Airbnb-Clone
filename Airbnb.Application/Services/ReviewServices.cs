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
        private readonly IUnitOfWork _unitOfWork;

        public ReviewServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddReviewAsync(Review review)
        {
            await _unitOfWork.Repository<Review, int>().AddAsync(review);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
            if (review != null)
            {
                _unitOfWork.Repository<Review, int>().Remove(review);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Review with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _unitOfWork.Repository<Review, int>().GetAllAsync();
        }

        public async Task<Review> GetReviewAsync(int id)
        {
            return await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(string propertyId)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null)
            {
                throw new KeyNotFoundException($"Property with ID {propertyId} not found.");
            }

            return property.Reviews;
        }

       
    }

}