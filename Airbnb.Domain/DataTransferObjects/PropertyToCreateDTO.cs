using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Airbnb.Domain.DataTransferObjects
{
    public class PropertyToCreateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? NightPrice { get; set; }
        public string? PlaceType { get; set; }
        public LocationDto? Location { get; set; }
        public RegionDto? Region { get; set; }
        public CountryDto? Country { get; set; }
        public OwnerDto? Owner { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }
        public IEnumerable<CategoryDto>? Categories { get; set; }
        public IEnumerable<RoomServicesDto>? RoomServices { get; set; }
    }
}
