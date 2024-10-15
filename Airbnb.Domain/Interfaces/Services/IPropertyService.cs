using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.DataTransferObjects.Property;
using Airbnb.Domain.Identity;

namespace Airbnb.Domain.Interfaces.Services
{
    public interface IPropertyService
    {
        Task<Responses> GetAllPropertiesAsync();
        Task<Responses> GetPropertyByIdAsync(string propertyId);
        Task<Responses> CreatePropertyAsync(PropertyRequest propertyDTO);
        Task<Responses> UpdatePropertyAsync(UpdatePropertyDto propertyDTO);
        Task<Responses> DeletePropertyAsync(string propertyId);

    }
}
