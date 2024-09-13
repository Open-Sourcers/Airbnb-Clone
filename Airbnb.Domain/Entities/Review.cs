using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Airbnb.Domain.Entities
{
	public class Review : BaseEntity<int>
	{
		public string Comment { get; set; } = string.Empty;
		public int Stars { get; set; }

		public virtual Property Property { get; set; }
		[ForeignKey("Property")]
		public string PropertyId { get; set; }

        public virtual AppUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
