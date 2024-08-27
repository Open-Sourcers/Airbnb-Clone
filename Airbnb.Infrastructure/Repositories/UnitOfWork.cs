using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AirbnbDbContext _context;
        private IGenericRepository<Booking, int> _bookingRepository;
        private IGenericRepository<Category, int> _categoryRepository;
        private IGenericRepository<Country, int> _countryRepository;
        private IGenericRepository<Image, int> _imageRepository;
        private IGenericRepository<Location, int> _locationRepository;
        private IGenericRepository<Property, int> _propertyRepository;
        private IGenericRepository<PropertyCategory, int> _propertyCategoryRepository;
        private IGenericRepository<Review, int> _reviewRepository;

        public UnitOfWork(AirbnbDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Booking, int> Bookings =>
            _bookingRepository ??= new GenericRepository<Booking, int>(_context);

        public IGenericRepository<Category, int> Categories =>
            _categoryRepository ??= new GenericRepository<Category, int>(_context);

        public IGenericRepository<Country, int> Countries =>
            _countryRepository ??= new GenericRepository<Country, int>(_context);

        public IGenericRepository<Image, int> Images =>
            _imageRepository ??= new GenericRepository<Image, int>(_context);

        public IGenericRepository<Location, int> Locations =>
            _locationRepository ??= new GenericRepository<Location, int>(_context);

        public IGenericRepository<Property, int> Properties =>
            _propertyRepository ??= new GenericRepository<Property, int>(_context);

        public IGenericRepository<PropertyCategory, int> PropertyCategories =>
            _propertyCategoryRepository ??= new GenericRepository<PropertyCategory, int>(_context);

        public IGenericRepository<Review, int> Reviews =>
            _reviewRepository ??= new GenericRepository<Review, int>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
