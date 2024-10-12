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
        Task<Responses> GetPropertyByIdAsync(string propertyId);
        Task<Responses> CreatePropertyAsync(string? email, PropertyToCreateDTO propertyDTO);
        Task<Responses> UpdatePropertyAsync(string propertyId, PropertyToUpdateDTO propertyDTO);
        Task<Responses> DeletePropertyAsync(string propertyId);

    }
}
