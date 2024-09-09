using Airbnb.Domain.DataTransferObjects;


namespace Airbnb.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Responses> Login(LoginDTO userDot);
        Task<Responses> Register(RegisterDTO user);
        Task<Responses> ResetPassword(ResetPasswordDTO resetPassword, string? email);
        Task<Responses> EmailConfirmation(string? email, string? code);
    }
}
