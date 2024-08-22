using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Image : BaseEntity<int>
	{
		public string Url { get; set; } = string.Empty;

		public virtual Property Property { get; set; }
		[ForeignKey("Property")]	
		public int PropertyId { get; set; }
		
	}
}
