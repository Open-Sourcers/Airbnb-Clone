
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
        public float Rate { get; set; }
        public IEnumerable<string>?RoomServices { get; set; } = new HashSet<string>();
        public IEnumerable<string>? Categories { get; set; }= new HashSet<string>();
    }
}
