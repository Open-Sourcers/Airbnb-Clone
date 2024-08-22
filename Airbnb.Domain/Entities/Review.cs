using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Review:BaseEntity<int>
	{
		public string Comment { get; set; } = string.Empty;
		public int Stars { get; set; }

		public virtual Property Property { get; set; }
		[ForeignKey("Property")]
		public int PropertyId { get; set; }

		// in complete user Relation
	}
}
