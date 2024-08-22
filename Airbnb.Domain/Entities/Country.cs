using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Country : BaseEntity<int>
	{
		public string Name { get; set; } = string.Empty;

		public virtual Region Region { get; set; }
		[ForeignKey("Region")]
		public int RegionId { get; set; }

		public virtual ICollection<Location> Locations { get; set; } = new HashSet<Location>(); 


	}
}
