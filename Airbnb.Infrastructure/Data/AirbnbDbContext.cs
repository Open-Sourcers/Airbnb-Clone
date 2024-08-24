using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Airbnb.Infrastructure.Data
{
    public class AirbnbDbContext : IdentityDbContext<AppUser>
    {
        public AirbnbDbContext(DbContextOptions<AirbnbDbContext> options) : base(options)
        {
             
        }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PropertyCategory>(E => {
                E.HasKey(K => new { K.CategoryId, K.PropertyId });
            });

            base.OnModelCreating(builder);
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyCategory> PropertyCategories { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet <RoomService> roomServices { get; set; }
    }
}
