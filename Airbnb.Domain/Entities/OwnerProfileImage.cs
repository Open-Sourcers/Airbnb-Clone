using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
    public class OwnerProfileImage:BaseEntity <int>
    {
        public string ImageUrl { get; set; }
        public int OwnerId { get; set; }
        public virtual Owner Owner { get; set; }

    }
}
