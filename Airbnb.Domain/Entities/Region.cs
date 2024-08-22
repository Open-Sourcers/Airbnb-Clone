using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Region:BaseEntity<int>
	{ 
		public string Name { get; set; } = string.Empty;
		public virtual ICollection<Country> Countries { get; set; }= new HashSet<Country>();
	}
}
