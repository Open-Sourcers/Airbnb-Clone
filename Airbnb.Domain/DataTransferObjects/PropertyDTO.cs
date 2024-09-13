using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Microsoft.AspNetCore.Http;

namespace Airbnb.Domain.DataTransferObjects
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set;}
    }
    public class RegionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class OwnerDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class PropertyDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NightPrice { get; set; }
        public string PlaceType { get; set; }
        public LocationDto Location { get; set;}
        public RegionDto Region { get; set; }
        public CountryDto Country { get; set; }
        public OwnerDto Owner { get; set; }
        public ICollection<IFormFile> Images { get; set; } = new HashSet<IFormFile>();
        public ICollection<CategoryDto> Categories { get; set; } = new HashSet<CategoryDto>();
        public ICollection<string> RoomServices { get; set; } = new List<string>();
    }
}
