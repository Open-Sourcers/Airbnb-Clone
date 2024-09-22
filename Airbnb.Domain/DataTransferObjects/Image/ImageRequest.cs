using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects.Image
{
    public class ImageRequest
    {
        public IFormFile? Image { get; set; }
    }
}
