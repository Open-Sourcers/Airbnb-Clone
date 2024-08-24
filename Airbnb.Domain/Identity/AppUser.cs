using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Property = Airbnb.Domain.Entities.Property;

namespace Airbnb.Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        [RegularExpression(@"^\d+-[A-Za-z]+-[A-Za-z]+-[A-Za-z]+$", ErrorMessage = "The address must follow the pattern '123-street-city-country'.")]
        public string Address { get; set; } = string.Empty;
        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public virtual ICollection <Property> Properties { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        [ForeignKey("Image")]

        public int? ImageId { get; set; }
        public virtual Image ProfileImage { get; set; }
    }
}
