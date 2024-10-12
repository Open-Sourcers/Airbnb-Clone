using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects
{
    public class CustomerDTO
    {
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
    }
}
