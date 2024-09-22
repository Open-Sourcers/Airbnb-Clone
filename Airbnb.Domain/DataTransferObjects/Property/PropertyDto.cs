using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.DataTransferObjects.Category;
using Airbnb.Domain.DataTransferObjects.Country;
using Airbnb.Domain.DataTransferObjects.Location;
using Airbnb.Domain.DataTransferObjects.Region;
using Airbnb.Domain.DataTransferObjects.User;

namespace Airbnb.Domain.DataTransferObjects.Property
{
    public class PropertyDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NightPrice { get; set; }
        public string PlaceType { get; set; }
        public string Country { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public IEnumerable<RoomServicesDto> RoomServices { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
    }
}
