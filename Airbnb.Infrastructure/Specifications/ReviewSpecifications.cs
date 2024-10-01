using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Interface;

namespace Airbnb.Infrastructure.Specifications
{
    public class ReviewSpecifications : BaseSpecifications<Review, int>
    {
        public ReviewSpecifications(string propertyId) : base(R => R.PropertyId == propertyId) 
        {
        }
    }
}
