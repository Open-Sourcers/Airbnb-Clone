using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;

namespace Airbnb.Domain.Interfaces.Services
{
    public interface IPropertyService
    {
        Task<Responses> GetAllPropertiesAsync();
        Task<Responses> GetPropertyByIdAsync(int propertyId);
        Task<Responses> CreatePropertyAsync(AppUser user, PropertyDTO propertyDTO);
        Task<Responses> UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO);
        Task<Responses> DeletePropertyAsync(int propertyId);

    }
}
