using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Identity;

namespace Airbnb.Domain.Entities
{
	public class Booking : BaseEntity<int>
	{
		//
		public decimal TotalPrice { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
		public string PaymentMethod { get; set; } = string.Empty;
		public DateTimeOffset PaymentDate { get; set; }

		public virtual Property Property { get; set; }
		[ForeignKey("Property")]
		public int PropertyId { get; set; }

        public virtual AppUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
