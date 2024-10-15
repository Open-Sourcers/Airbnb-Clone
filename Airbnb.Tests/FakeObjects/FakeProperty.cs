using Airbnb.Domain.Entities;
using Bogus;

namespace Airbnb.Tests.FakeObjects
{
    internal class FakeProperty:Faker<Property>
    {
        public FakeProperty()
        {
            RuleFor(x => x.Id, f => Guid.NewGuid().ToString());
            RuleFor(x => x.Name, f => f.Name.FullName());
            RuleFor(x => x.Description, f => f.Lorem.Sentence());
            RuleFor(x => x.NightPrice, f => f.Finance.Amount(50, 500));  
            RuleFor(x => x.Rate, f => f.Random.Float(1, 5));  
            RuleFor(x => x.PlaceType, f => f.Commerce.ProductAdjective());
            RuleFor(x => x.LocationId, f => f.Random.Int(1, 100));  
            RuleFor(x => x.OwnerId, f => f.Random.Guid().ToString());  

            Ignore(x => x.Location);
            Ignore(x => x.Owner);
            Ignore(x => x.Images);
            Ignore(x => x.Categories);
            Ignore(x => x.Reviews);
            Ignore(x => x.RoomServices);
            Ignore(x => x.Bookings);
        }
    }
}
