using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Airbnb.Domain.Identity;
namespace Airbnb.Domain.Entities
{
    public class Booking : BaseEntity<int>
    {
        public decimal TotalPrice { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTimeOffset BookingDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset PaymentDate { get; set; }
        public string PaymentStatus { get; set; } =string.Empty;
        public string PaymentIntentId { get; set; }

        [ForeignKey("Property")]
        public string PropertyId { get; set; }
        public virtual Property Property { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

    }
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum PaymentMethod
    {
        [EnumMember(Value = "Visa")]
        Visa,
        [EnumMember(Value = "Cache")]
        Cache,
        [EnumMember(Value = "Stripe")]
        Stripe
    }
    public enum PaymentStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
