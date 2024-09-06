using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        public async Task<Responses> Login(LoginDTO userDto)
        {
            var user =await  _userManager.FindByEmailAsync(userDto.Email);
            if (user ==null) {
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
                    return await Responses.SuccessResponse(await _authService.CreateTokenAsync(user,_userManager), "Success");
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
            var IsCreated =await _userManager.CreateAsync(account, account.PasswordHash);
            if (!IsCreated.Succeeded)
            {
                return await Responses.FailurResponse(IsCreated.Errors);
            }
            else
            {
                return await Responses.SuccessResponse(await _authService.CreateTokenAsync(account, _userManager), "Account Has Been Created Successfully");
            }
        }
    }
}
