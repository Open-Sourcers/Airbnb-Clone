using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Category : BaseEntity<int>
	{
		public virtual ICollection<Property> Properties { get; set; }=new HashSet<Property>();

	}
}
