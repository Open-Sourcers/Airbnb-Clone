using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Identity;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Tests.FakeObjects
{
    internal class FakeRegisterDto:Faker<RegisterDTO>
    {
        public FakeRegisterDto()
        {
            RuleFor(x => x.FirstName, f => f.Name.FirstName());
            RuleFor(x => x.MiddlName, f => f.Name.FirstName());
            RuleFor(x => x.LastName, f => f.Name.LastName());
            RuleFor(x => x.Address, f => $"{f.Address.BuildingNumber()} {f.Address.StreetName()}, {f.Address.City()}, {f.Address.Country()}");
            RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber());
            RuleFor(x => x.Email, f => f.Internet.Email());
            RuleFor(x => x.UserName, f => f.Internet.UserName());
            RuleFor(x => x.Password, f => f.Internet.Password());
        }
    }
}
