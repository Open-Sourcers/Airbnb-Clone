using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;  // For HttpContext and User
using System.Security.Claims;
namespace Airbnb.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Responses> Login(LoginDTO userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                return await Responses.FailurResponse("Your Email Is Not Found!.");
            }
            else
            {
                var loginSuccess = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
                if (!loginSuccess.Succeeded)
                {
                    return await Responses.FailurResponse("InCorrect Password!.");
                }
                else
                {
                    return await Responses.SuccessResponse(await _authService.CreateTokenAsync(user, _userManager), "Success");
                }
            }
        }

        public async Task<Responses> Register(RegisterDTO user)
        {
            var account = new AppUser()
            {
                FullName = $"{user.FirstName} {user.MiddltName} {user.LastName}",
                Address = user.Address,
                Email = user.Email,
                UserName = user.Email.Split('@')[0],
                PasswordHash = user.Password,
                PhoneNumber = user.PhoneNumber,
                ProfileImage = "Built Upload Image To User Image URL",

            };
            var IsCreated = await _userManager.CreateAsync(account, account.PasswordHash);
            if (!IsCreated.Succeeded)
            {
                return await Responses.FailurResponse(IsCreated.Errors);
            }
            else
            {
                var roles = new List<string> { user.role.ToString() };
                var addToRolesResult = await _userManager.AddToRolesAsync(account, roles);
                if (!addToRolesResult.Succeeded)
                {
                    return await Responses.FailurResponse(addToRolesResult.Errors);
                }
                return await Responses.SuccessResponse(await _authService.CreateTokenAsync(account, _userManager), "Account Has Been Created Successfully");
            }
        }

        public async Task<Responses> ResetPassword(ResetPasswordDTO resetPassword)
        {
           
            var userClaim = _httpContextAccessor.HttpContext?.User;
            if (userClaim == null || !userClaim.Identity.IsAuthenticated)
            {
                return await Responses.FailurResponse("User is not authenticated.");
            }

            string Email = userClaim.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _userManager.FindByEmailAsync(Email);
            if (user.PasswordHash != resetPassword.CurrentPassword)
            {
                return await Responses.FailurResponse("Your Password in Correct!.");
            }
            user.PasswordHash = resetPassword.NewPassword;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return await Responses.FailurResponse("Un Expected Error Try Again.");
            }
            return await Responses.SuccessResponse(Email, "Your Password Updated Successfully.");
        }
    } 
}
