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
        Task<Responses> GetAllReviewsAsync(string? propertyId,string? userId);
        Task<Responses> AddReviewAsync(ReviewDto review); 
        Task<Responses> UpdateReviewAsync(string userId,int id,ReviewDto review); 
        Task<Responses> DeleteReviewAsync(int id);

    }
}
