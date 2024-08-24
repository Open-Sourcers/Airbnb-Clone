using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Property : BaseEntity<int>
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal NightPrice { get; set; }
		public float Rate { get; set; }
		public string PlaceType { get; set; } = string.Empty;

		public virtual Location Location { get; set; }
		[ForeignKey("Location")]
		public int LocationId { get; set; }
        [ForeignKey("Owner")]

        public int OwnerId { get; set; }
		public virtual Owner Owner { get; set; }

		public virtual ICollection<PropertyImage> Images { get; set; } = new HashSet<PropertyImage>();

		public virtual ICollection<PropertyCategory> Categories { get; set; } = new HashSet<PropertyCategory>();

		public virtual ICollection<Review> Reviews { get; set; }=new HashSet<Review>();


        public virtual ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
	}
}
