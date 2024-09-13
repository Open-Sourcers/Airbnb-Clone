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
        public string Decscription {  get; set; } = string.Empty;
        [ForeignKey("Property")]
        public string PropertyId { get; set; }
        public virtual Property property { get; set; }


    }
}
