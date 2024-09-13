using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;

namespace Airbnb.Domain.DataTransferObjects
{
    public class PropertyDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NightPrice { get; set; }
        public string PlaceType { get; set; }
        public Location Location { get; set; }
        public OwnerDTO Owner { get; set; }
        public ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public ICollection<PropertyCategory> Categories { get; set; } = new HashSet<PropertyCategory>();
        public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
