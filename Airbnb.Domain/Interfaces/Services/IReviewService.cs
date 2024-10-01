using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;

namespace Airbnb.Domain.Interfaces.Services
{
    public interface IReviewService
    {
        Task<Responses> GetReviewAsync(int id);
        Task<Responses> AddReviewAsync(string? email, ReviewDTO review);
        Task<Responses> GetReviewsByPropertyIdAsync(string propertyId);
        Task<Responses> DeleteReviewAsync(int id);
        Task<Responses> UpdateReviewAsync(string? email, int id, ReviewDTO reviewDTO);
    }
}
