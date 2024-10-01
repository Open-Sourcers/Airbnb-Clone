using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Identity;

namespace Airbnb.Domain.DataTransferObjects
{
    public class ReviewDTO
    {
        public string Comment { get; set; } = string.Empty;
        public int Stars { get; set; }
        public string PropertyId { get; set; }
    }
}
