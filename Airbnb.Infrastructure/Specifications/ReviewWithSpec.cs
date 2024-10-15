using Airbnb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Infrastructure.Specifications
{
    public class ReviewWithSpec : BaseSpecifications<Review, int>
    {
        public ReviewWithSpec(string? propertyId, string? userId) : base(P =>
        (string.IsNullOrWhiteSpace(propertyId) || P.PropertyId == propertyId)
        && (string.IsNullOrWhiteSpace(userId) || P.UserId == userId))

        {
            AddOrderByDescending(x => x.Stars);
        }
    }
}
