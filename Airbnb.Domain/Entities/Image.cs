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
		public virtual Property Property { get; set; }
		[ForeignKey("Property")]	
		public string PropertyId { get; set; }
		
	}
}
