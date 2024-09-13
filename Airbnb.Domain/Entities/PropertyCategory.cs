using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class PropertyCategory : BaseEntity<int>
	{
		public virtual Category Category { get; set; }
		[ForeignKey("Category")]
		public int CategoryId { get; set; }


		public virtual Property Property { get; set; }
		[ForeignKey("Property")]
		public string PropertyId { get; set; }
	}
}
