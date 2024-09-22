using System.ComponentModel.DataAnnotations.Schema;

namespace Airbnb.Domain.Entities
{
	public class Country : BaseEntity<int>
	{
		public virtual Region Region { get; set; }
		[ForeignKey("Region")]
		public int RegionId { get; set; }

		public virtual ICollection<Location> Locations { get; set; } = new HashSet<Location>(); 


	}
}
