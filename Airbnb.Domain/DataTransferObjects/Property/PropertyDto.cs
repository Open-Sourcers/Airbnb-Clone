
using Airbnb.Domain.DataTransferObjects.Category;


namespace Airbnb.Domain.DataTransferObjects.Property
{
    public class PropertyDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? NightPrice { get; set; }
        public string? PlaceType { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? Region { get; set; }
        public List<string>?RoomServices { get; set; } = new List<string>();
        public List<string>? Categories { get; set; }= new List<string>();
    }
}
