using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;

namespace Airbnb.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<Booking, int> Bookings { get; }
        IGenericRepository<Category, int> Categories { get; }
        IGenericRepository<Country, int> Countries { get; }
        IGenericRepository<Image, int> Images { get; }
        IGenericRepository<Location, int> Locations { get; }
        IGenericRepository<Property, int> Properties { get; }
        IGenericRepository<PropertyCategory, int> PropertyCategories { get; }
        IGenericRepository<Review, int> Reviews { get; }
    }
}
