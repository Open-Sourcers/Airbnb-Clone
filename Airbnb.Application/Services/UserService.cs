using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
using Microsoft.AspNetCore.Http;  // For HttpContext and User
using System.Security.Claims;
=======
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

>>>>>>> c49054459ef8de84058b2f4790d3b8bd3c1cc5f7
namespace Airbnb.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
<<<<<<< HEAD
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService, IHttpContextAccessor httpContextAccessor)
=======
        private readonly IMailService _mailService;

        public UserService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           IAuthService authService,
                           IMailService mailService)
>>>>>>> c49054459ef8de84058b2f4790d3b8bd3c1cc5f7
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
<<<<<<< HEAD
            _httpContextAccessor = httpContextAccessor;
=======
            _mailService = mailService;
>>>>>>> c49054459ef8de84058b2f4790d3b8bd3c1cc5f7
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
                var IsConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if(!IsConfirmed) return await Responses.FailurResponse("Email is not confirmed yet!");
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

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(account);
                var message = $"please verify you email by this code {code}";
                await _mailService.SendEmailAsync(user.Email, "Email Confirmation", message);

                return await Responses.SuccessResponse(await _authService.CreateTokenAsync(account, _userManager), $"Please confirm your email address! code {code}");
            }
        }

<<<<<<< HEAD
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
=======
        public async Task<Responses> EmailConfirmation(string? email, string? code)
        {
            if (email == null || code == null) return await Responses.FailurResponse("invalid payloads");

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null) return await Responses.FailurResponse("invalid payloads");

            var Isverified = await _userManager.ConfirmEmailAsync(user, code);
            if(!Isverified.Succeeded) return await Responses.FailurResponse(Isverified.Errors);

            return await Responses.SuccessResponse("Email has been confirmed.");
        }

    }
>>>>>>> c49054459ef8de84058b2f4790d3b8bd3c1cc5f7
}
