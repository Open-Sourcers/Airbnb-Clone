using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Location : BaseEntity<int>
	{
		public virtual Country Country { get; set; }
		[ForeignKey("Country")]
		public int CountryId { get; set; }
		public virtual ICollection<Property> Properties { get; set; } = new HashSet<Property>();
	}
}
