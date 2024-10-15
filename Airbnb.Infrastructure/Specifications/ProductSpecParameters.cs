using Microsoft.VisualBasic;
using System.Text.Json.Serialization;

namespace Airbnb.Infrastructure.Specifications
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Sort
    {
        RateAsc,
        RateDesc,
        NightPriceAsc,
        NightPriceDesc,
        Name
    }
    public class DateRange
    {
        
    }
    public class ProductSpecParameters
    {
        public Sort? sort { get; set; }
        public string? categoryName { get; set; }
        public int? locationId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? ednDate { get; set; }
        public int pageIndex { get; set; }
        private int pageSize = 20;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > 100 || pageSize < 1) ? 20 : value; }
        }
    }

}

