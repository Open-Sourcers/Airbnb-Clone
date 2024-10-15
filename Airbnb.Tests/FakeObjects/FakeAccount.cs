using Airbnb.Domain.Identity;
using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Org.BouncyCastle.Crypto.Fpe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Tests.FakeObjects
{
    internal class FakeAccount : Faker<AppUser>
    {
        public FakeAccount()
        {
            RuleFor(x => x.FullName, f => f.Name.FullName());
            RuleFor(x => x.Address, f => $"{f.Address.BuildingNumber()} {f.Address.StreetName()}, {f.Address.City()}, {f.Address.Country()}");
            RuleFor(x => x.Email, f => f.Internet.Email());
            RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber());
            RuleFor(x => x.UserName, f => f.Internet.UserName());
        }
    }
}
