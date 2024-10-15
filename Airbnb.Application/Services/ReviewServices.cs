using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Airbnb.Application.Services
{
    public class ReviewServices : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public ReviewServices(IUnitOfWork unitOfWork, IMapper mapper, IReviewRepository reviewRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        public async Task<Responses> AddReviewAsync(ReviewDto review)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(review.PropertyId);
            if (property == null)
            {
                return await Responses.FailurResponse($"Not found property with Id {review.PropertyId}!");
            }
            try
            {
                await _unitOfWork.Repository<Review, int>().AddAsync(_mapper.Map<Review>(review));
                await _unitOfWork.CompleteAsync();
                var countAndSum = await _reviewRepository.CountReviewsAdnSumStars(review.PropertyId);
                property.Rate = (countAndSum.Item2 / (float)countAndSum.Item1);

                _unitOfWork.Repository<Property, string>().Update(property);
                await _unitOfWork.CompleteAsync();
                return await Responses.SuccessResponse("Commend added successfully!");
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Responses> DeleteReviewAsync(int id)
        {
            var review = await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
            if (review == null)
            {
                return await Responses.FailurResponse($"InValid Id {id}!", HttpStatusCode.InternalServerError);
            }
            try
            {
                _unitOfWork.Repository<Review, int>().Remove(review);
                await _unitOfWork.CompleteAsync();
                return await Responses.SuccessResponse($"Review has been deleted successfully!");
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Responses> GetAllReviewsAsync(string? propertyId, string? userId)
        {
            var spec = new ReviewWithSpec(propertyId, userId);
            var reviews = await _unitOfWork.Repository<Review, int>().GetAllWithSpecAsync(spec);

            if (reviews == null)
            {
                return await Responses.FailurResponse("No reviews found.");
            }
            return await Responses.SuccessResponse(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
        }

        public async Task<Responses> GetReviewAsync(int id)
        {

            var review= await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
            if (review == null)
            {
                return await Responses.FailurResponse($"Not found review with id {id}");
            }
            return await Responses.SuccessResponse(_mapper.Map<ReviewDto>(review));
        }

        public async Task<Responses> UpdateReviewAsync(string userId,int id,ReviewDto review)
        {
            var existingReview = await _unitOfWork.Repository<Review,int>().GetByIdAsync(id);
            if (existingReview == null)
            {
                return await Responses.FailurResponse($"Review with ID {id} not found.",HttpStatusCode.NotFound);
            }
            if(userId!= existingReview.UserId)
            {
                return await Responses.FailurResponse($"UnAuthorized user.", HttpStatusCode.Unauthorized);

            }
            var maped = _mapper.Map<Review>(review);

             _unitOfWork.Repository<Review,int>().Update(maped);
            await _unitOfWork.CompleteAsync();
            return await Responses.SuccessResponse(review, "Review updated successfully.");
        }
    }

}