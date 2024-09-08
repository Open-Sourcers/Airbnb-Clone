using Airbnb.Domain.DataTransferObjects;


namespace Airbnb.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Responses> Login(LoginDTO userDot);
        Task<Responses> Register(RegisterDTO user);
<<<<<<< HEAD
        Task<Responses> ResetPassword(ResetPasswordDTO resetPassword);

=======
        Task<Responses> EmailConfirmation(string? email, string? code);
>>>>>>> c49054459ef8de84058b2f4790d3b8bd3c1cc5f7
    }
}
