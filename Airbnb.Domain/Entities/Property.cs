using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Identity;

namespace Airbnb.Domain.Entities
{
	public class Property : BaseEntity<string>
	{
		public string Description { get; set; }
		public decimal NightPrice { get; set; }
		public float Rate { get; set; }
		public string PlaceType { get; set; } = string.Empty;

		[ForeignKey("Location")]
		public int LocationId { get; set; }
		public virtual Location Location { get; set; }

		[ForeignKey("Owner")]
		public string OwnerId { get; set; }
		public virtual AppUser Owner { get; set; }

		public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();

		public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

		public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

        public virtual ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
	}
}
