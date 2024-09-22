using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
    public class RoomService:BaseEntity<int>
    {

        [ForeignKey("Property")]
        public string PropertyId { get; set; }
        public virtual Property property { get; set; }

    }
}
