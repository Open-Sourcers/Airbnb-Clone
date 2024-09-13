using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;  // For HttpContext and User
using System.Security.Claims;
using System.Net;
using AutoMapper;
//using Microsoft.AspNetCore.Identity;


namespace Airbnb.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           IAuthService authService,
                           IMailService mailService,
                           IHttpContextAccessor httpContextAccessor,
                           IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<Responses> Login(LoginDTO userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                return await Responses.FailurResponse("Your Email Is Not Found!.", HttpStatusCode.BadRequest);
            }
            else
            {
                var IsConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!IsConfirmed) return await Responses.FailurResponse("Email is not confirmed yet!", HttpStatusCode.BadRequest);
                var loginSuccess = await _signInManager.PasswordSignInAsync(user, userDto.Password,true,true);
                if (!loginSuccess.Succeeded)
                {
                    return await Responses.FailurResponse("InCorrect Password!.", HttpStatusCode.BadRequest);
                }
                else
                {
                    return await Responses.SuccessResponse(await _authService.CreateTokenAsync(user, _userManager), "Success");
                }
            }
            
        }

        public async Task<Responses> Register(RegisterDTO user)
        {
            var isFound = await _userManager.FindByEmailAsync(user.Email);
            if (isFound is not null) return await Responses.FailurResponse("User Is Already Exist!.", HttpStatusCode.InternalServerError);
            var account = new AppUser()
            {
                FullName = $"{user.FirstName} {user.MiddlName} {user.LastName}",
                Address = user.Address,
                Email = user.Email,
                UserName = (user.UserName is null)? user.Email.Split('@')[0] : user.UserName,
                PasswordHash = user.Password,
                PhoneNumber = user.PhoneNumber,
                ProfileImage = "Built Upload Image To User Image URL",

            };
            var IsCreated = await _userManager.CreateAsync(account, account.PasswordHash);
            if (!IsCreated.Succeeded)
            {
                return await Responses.FailurResponse(IsCreated.Errors, HttpStatusCode.InternalServerError);
            }
            else
            {
                var roles = user.roles.Select(role => role.ToString()).ToList();
                var addToRolesResult = await _userManager.AddToRolesAsync(account, roles);
                if (!addToRolesResult.Succeeded)
                {
                    return await Responses.FailurResponse(addToRolesResult.Errors, HttpStatusCode.InternalServerError);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(account);
                var message = $"please verify you email by this code {code}";
                await _mailService.SendEmailAsync(user.Email, "Email Confirmation", message);

                return await Responses.SuccessResponse(await _authService.CreateTokenAsync(account, _userManager), $"Please confirm your email address! code {code}");
            }
        }

        public async Task<Responses> ResetPassword(ResetPasswordDTO resetPassword, string? email)
        {
            if (email is null)
            {
                return await Responses.FailurResponse(HttpStatusCode.NotFound);
            }

            var user = await _userManager.FindByEmailAsync(email);
            
            if (!await _userManager.CheckPasswordAsync(user,resetPassword.CurrentPassword))
            {
                return await Responses.FailurResponse("Your Password in Correct!.");
            }
            // Exception Here
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,token,resetPassword.NewPassword);
            if (!result.Succeeded)
            {
                return await Responses.FailurResponse("Un Expected Error Try Again.",HttpStatusCode.InternalServerError);
            }
            return await Responses.SuccessResponse(email, "Your Password Updated Successfully.");
        }
        public async Task<Responses>UpdateUser(AppUser user,UpdateUserDTO userDTO)
        {
            string FullName = userDTO.FirstName;
            if(userDTO.MiddlName is not null)
            {
                FullName += (" " + userDTO.MiddlName);
            }
            FullName += (" " + userDTO.LastName);

            user.FullName = FullName;
            user.Email = userDTO.Email;
            user.UserName = userDTO.UserName;
            user.Address = userDTO.Address;
            user.PhoneNumber = userDTO.PhoneNumber;
            
            var IsUpdatedSuccessfully = await _userManager.UpdateAsync(user);
            if(!IsUpdatedSuccessfully.Succeeded)
            {
                return await Responses.FailurResponse(IsUpdatedSuccessfully.Errors);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var message = $"please verify you email by this code {code}";
            await _mailService.SendEmailAsync(user.Email, "Email Confirmation", message);
            return await Responses.SuccessResponse("User Updated Successfully!.");

        }
        public async Task<Responses> EmailConfirmation(string? email, string? code)
        {
            if (email == null || code == null) return await Responses.FailurResponse("invalid payloads");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return await Responses.FailurResponse("invalid payloads", HttpStatusCode.NotFound);

            var Isverified = await _userManager.ConfirmEmailAsync(user, code);
            if (!Isverified.Succeeded) return await Responses.FailurResponse(Isverified.Errors, HttpStatusCode.InternalServerError);

            return await Responses.SuccessResponse("Email has been confirmed.");
        }

        public async Task<Responses> CreateUserAsync(RegisterDTO userDto)
        {
            var user = _mapper.Map<AppUser>(userDto);
            user.ProfileImage = "Needed To Create";
            var IsCreated = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!IsCreated.Succeeded)
            {
                return await Responses.FailurResponse(IsCreated.Errors, HttpStatusCode.InternalServerError);
            }
            else
            {
                var roles = userDto.roles.Select(role => role.ToString()).ToList();
                var addToRolesResult = await _userManager.AddToRolesAsync(user, roles);
                if (!addToRolesResult.Succeeded)
                {
                    return await Responses.FailurResponse(addToRolesResult.Errors, HttpStatusCode.InternalServerError);
                }
                return await Responses.SuccessResponse(userDto, "User Created Successfully!.");
            }
        }
    }
}
