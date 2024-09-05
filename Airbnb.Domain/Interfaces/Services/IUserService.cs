using Airbnb.Domain.DataTransferObjects;


namespace Airbnb.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Responses> Login(LoginDTO userDot);
        Task<Responses> Register(RegisterDTO user);
        

    }
}
