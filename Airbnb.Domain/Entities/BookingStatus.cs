using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
    public enum BookingStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived,

        [EnumMember(Value = "PaymentFailded")]
        PaymentFailded,

        [EnumMember(Value = "Canceled")]
        Canceled
    }
}
