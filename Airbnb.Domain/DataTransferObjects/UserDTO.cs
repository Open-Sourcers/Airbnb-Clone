using Airbnb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
        public IEnumerable<Property> Properties { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        // profileImage": "Built Upload Image To User Image URL",
    
    }
}
