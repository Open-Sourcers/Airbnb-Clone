using System.ComponentModel.DataAnnotations.Schema;
using Airbnb.Domain.Identity;
namespace Airbnb.Domain.Entities
{
	public class Booking : BaseEntity<int>
	{
		public decimal TotalPrice { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
		public string PaymentMethod { get; set; } = string.Empty;
		public DateTimeOffset PaymentDate { get; set; }

        [ForeignKey("Property")]
        public string PropertyId { get; set; }
        public virtual Property Property { get; set; }
		

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
