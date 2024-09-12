using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;


namespace Airbnb.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Responses> Login(LoginDTO userDot);
        Task<Responses> Register(RegisterDTO user);
        Task<Responses> ResetPassword(ResetPasswordDTO resetPassword, string? email);
        Task<Responses> UpdateUser(AppUser user, UpdateUserDTO userDTO);
        Task<Responses> CreateUserAsync(RegisterDTO userDto);
        Task<Responses> EmailConfirmation(string? email, string? code);
    }
}
