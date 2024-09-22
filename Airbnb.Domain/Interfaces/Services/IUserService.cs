using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Identity;


namespace Airbnb.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Responses> Login(LoginDTO userDot);
        Task<Responses> Register(RegisterDTO user);
        Task<Responses> ResetPassword(ResetPasswordDTO resetPassword);
        Task<Responses> UpdateUser(AppUser user, UpdateUserDTO userDTO);
        Task<Responses> CreateUserAsync(RegisterDTO userDto);
        Task<Responses> EmailConfirmation(string? email, string? code);
        Task<Responses> ForgetPassword(ForgetPasswordDto forgetPassword);
        Task<Responses> RemoveUser(AppUser user);
    }
}
