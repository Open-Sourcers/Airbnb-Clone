using System.Text.Json;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Airbnb.Infrastructure.Data
{
    public class DataInitializer
    {
        public static async Task SeedAsync(AirbnbDbContext context, RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> { "Admin", "Owner", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (!context.Users.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Users.json";
                var UsersData = File.ReadAllText(path);
                var users = JsonSerializer.Deserialize<List<AppUser>>(UsersData);

                if (users.Count() > 0)
                {
                    foreach (var user in users)
                    {
                        await context.Set<AppUser>().AddAsync(user);
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!context.Regions.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Regions.json";
                var RegionsData = File.ReadAllText(path);
                var regions = JsonSerializer.Deserialize<List<Region>>(RegionsData);

                if (regions.Count() > 0)
                {
                    foreach (var region in regions)
                    {
                        await context.Set<Region>().AddAsync(region);
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!context.Countries.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Countries.json";
                var CountriesData = File.ReadAllText(path);
                var countries = JsonSerializer.Deserialize<List<Country>>(CountriesData);

                if (countries.Count() > 0)
                {
                    foreach (var country in countries)
                    {
                        await context.Set<Country>().AddAsync(country);
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!context.Locations.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Locations.json";
                var LocationsData = File.ReadAllText(path);
                var locations = JsonSerializer.Deserialize<List<Location>>(LocationsData);

                if (locations.Count() > 0)
                {
                    foreach (var location in locations)
                    {
                        await context.Set<Location>().AddAsync(location);
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!context.Properties.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Properties.json";
                var PropertiesData = File.ReadAllText(path);
                var properties = JsonSerializer.Deserialize<List<Property>>(PropertiesData);

                if (properties.Count() > 0)
                {
                    foreach (var property in properties)
                    {
                        await context.Set<Property>().AddAsync(property);
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!context.Bookings.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Bookings.json";
                var BookingsData = File.ReadAllText(path);
                var bookings = JsonSerializer.Deserialize<List<Booking>>(BookingsData);

                if (bookings.Count() > 0)
                {
                    foreach (var booking in bookings)
                    {
                        await context.Set<Booking>().AddAsync(booking);
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!context.Reviews.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Reviews.json";
                var ReviewsData = File.ReadAllText(path);
                var reviews = JsonSerializer.Deserialize<List<Review>>(ReviewsData);

                if (reviews.Count() > 0)
                {
                    foreach (var review in reviews)
                    {
                        await context.Set<Review>().AddAsync(review);
                    }
                }

                await context.SaveChangesAsync();
            }
            //TODO: Solve Data Seeding Problem

            if (!context.Categories.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Categories.json";
                var CategoriesData = File.ReadAllText(path);
                var categories = JsonSerializer.Deserialize<List<Category>>(CategoriesData);

                if (categories.Count() > 0)
                {
                    foreach (var category in categories)
                    {
                        await context.Set<Category>().AddAsync(category);
                    }
                }

                await context.SaveChangesAsync();
            }

            //if (!context.PropertyCategories.Any())
            //{
            //    string path = "../Airbnb.Infrastructure/Data/DataSeed/PropertiesCategories.json";
            //    var PropertyCategoriesData = File.ReadAllText(path);
            //    var propertyCategories = JsonSerializer.Deserialize<List<PropertyCategory>>(PropertyCategoriesData);

            //    if (propertyCategories.Count() > 0)
            //    {
            //        foreach (var propertyCategory in propertyCategories)
            //        {
            //            await context.Set<PropertyCategory>().AddAsync(propertyCategory);
            //        }
            //    }
            //    await context.SaveChangesAsync();
            //}

            if (!context.Images.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/Images.json";
                var ImagesData = File.ReadAllText(path);
                var images = JsonSerializer.Deserialize<List<Image>>(ImagesData);

                if (images.Count() > 0)
                {
                    foreach (var image in images)
                    {
                        await context.Set<Image>().AddAsync(image);
                    }
                }

                var x=await context.SaveChangesAsync();
            }


            if (!context.roomServices.Any())
            {
                string path = "../Airbnb.Infrastructure/Data/DataSeed/RoomServices.json";
                var roomServicesData = File.ReadAllText(path);
                var RoomServices = JsonSerializer.Deserialize<List<RoomService>>(roomServicesData);

                if (RoomServices.Count() > 0)
                {
                    foreach (var roomService in RoomServices)
                    {
                        await context.Set<RoomService>().AddAsync(roomService);
                    }
                }

                await context.SaveChangesAsync();
            }

        }
    }
}
