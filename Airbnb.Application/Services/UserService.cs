using Airbnb.Domain;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;  // For HttpContext and User
using System.Security.Claims;
using System.Net;
using AutoMapper;
using System.Reflection.Metadata;
using Airbnb.Application.Settings;
using Airbnb.Application.Utility;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Entities;
using Airbnb.Infrastructure.Specifications;
using Airbnb.Domain.DataTransferObjects.User;
//using Microsoft.AspNetCore.Identity;


namespace Airbnb.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           IAuthService authService,
                           IMailService mailService,
                           IMapper mapper,
                           IMemoryCache memoryCache,
                           IUnitOfWork unitOfWork)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mailService = mailService;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<Responses> Login(LoginDTO userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                return await Responses.FailurResponse("Your email is not found!.", HttpStatusCode.BadRequest);
            }
            else
            {
                var IsConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!IsConfirmed) return await Responses.FailurResponse("Email is not confirmed yet!", HttpStatusCode.BadRequest);
                var loginSuccess = await _signInManager.PasswordSignInAsync(user, userDto.Password, true, true);
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
            if (isFound is not null) return await Responses.FailurResponse("User Is Already Exist!.", HttpStatusCode.BadRequest);

            var account = new AppUser()
            {
                FullName = $"{user.FirstName} {user.MiddlName} {user.LastName}",
                Address = user.Address,
                Email = user.Email,
                UserName = (user.UserName is null) ? user.Email.Split('@')[0] : user.UserName,
                PasswordHash = user.Password,
                PhoneNumber = user.PhoneNumber,

            };

            if (user.Image != null)
            {
                account.ProfileImage = await DocumentSettings.UploadFile(user.Image, SD.Image, SD.UserProfile);
            }

            var IsCreated = await _userManager.CreateAsync(account, account.PasswordHash);
            if (!IsCreated.Succeeded)
            {
                await DocumentSettings.DeleteFile(SD.Image, SD.UserProfile, account.ProfileImage);
                return await Responses.FailurResponse(IsCreated.Errors, HttpStatusCode.InternalServerError);
            }
            else
            {

                var roles = user.roles.Select(role => role.ToString()).ToList();
                if (roles.Count() == 0)
                {
                    await DocumentSettings.DeleteFile(SD.Image, SD.UserProfile, account.ProfileImage);
                    return await Responses.FailurResponse("Can't create account without roles", HttpStatusCode.InternalServerError);
                }

                try
                {
                    await _userManager.AddToRolesAsync(account, roles);
                }
                catch (Exception ex)
                {
                    await DocumentSettings.DeleteFile(SD.Image, SD.UserProfile, account.ProfileImage);
                    return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(account);
                var message = $"please verify you email by this code {code}";
                await _mailService.SendEmailAsync(user.Email, "Email Confirmation", message);
                return await Responses.SuccessResponse(await _authService.CreateTokenAsync(account, _userManager), $"Please confirm your email address! code {code}");
            }
        }

        public async Task<Responses> UpdateUser(AppUser user, UpdateUserDTO userDTO)
        {
            string FullName = userDTO.FirstName;
            if (userDTO.MiddlName is not null)
            {
                FullName += (" " + userDTO.MiddlName);
            }
            FullName += (" " + userDTO.LastName);

            user.FullName = FullName;
            user.Email = userDTO.Email;
            user.UserName = userDTO.UserName;
            user.Address = userDTO.Address;
            user.PhoneNumber = userDTO.PhoneNumber;
            string? oldImage = user.ProfileImage;
            if (userDTO.ProfileImage != null)
            {
                user.ProfileImage = await DocumentSettings.UploadFile(userDTO.ProfileImage, SD.Image, SD.UserProfile);
            }
            var IsUpdatedSuccessfully = await _userManager.UpdateAsync(user);
            if (!IsUpdatedSuccessfully.Succeeded)
            {
                if (user.ProfileImage != null)
                {
                    await DocumentSettings.DeleteFile(SD.Image, SD.UserProfile, user.ProfileImage);
                }

                return await Responses.FailurResponse(IsUpdatedSuccessfully.Errors);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var message = $"please verify you email by this code {code}";
            await _mailService.SendEmailAsync(user.Email, "Email Confirmation", message);

            if (oldImage != null)
            {
                await DocumentSettings.DeleteFile(SD.Image, SD.UserProfile, oldImage);
            }

            return await Responses.SuccessResponse("User updated successfully!.");

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
            if (userDto.Image != null)
            {
                user.ProfileImage = await DocumentSettings.UploadFile(userDto.Image, SD.Image, SD.UserProfile);
            }
            var IsCreated = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!IsCreated.Succeeded)
            {
                if (user.ProfileImage != null)
                {
                    await DocumentSettings.DeleteFile(SD.Image, SD.UserProfile, user.ProfileImage);
                }
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
                var createduser = await _userManager.FindByEmailAsync(userDto.Email);
                var result = _mapper.Map<UserDTO>(createduser);
                return await Responses.SuccessResponse(result, "User created successfully!.");
            }
        }

        public async Task<Responses> ForgetPassword(ForgetPasswordDto forgetPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgetPassword.Email);
            if (user == null)
            {
                return await Responses.FailurResponse("Email is not found!", HttpStatusCode.BadRequest);
            }

            var otp = new Random().Next(100000, 999999).ToString();
            _memoryCache.Set(user.Email, otp, TimeSpan.FromMinutes(10));

            await _mailService.SendEmailAsync(user.Email, "Reset password!", otp);

            return await Responses.SuccessResponse(Token: await _userManager.GeneratePasswordResetTokenAsync(user), message: "Check your mail!");
        }
        public async Task<Responses> ResetPassword(ResetPasswordDTO resetPassword)
        {

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return await Responses.FailurResponse("Email is not found!");
            }

            if (!_memoryCache.TryGetValue(user.Email, out string Otp))
            {
                return await Responses.FailurResponse("Time expired.... try again!", HttpStatusCode.BadRequest);
            }

            if (Otp != resetPassword.Otp)
            {
                return await Responses.FailurResponse("Invalid Otp!", HttpStatusCode.BadRequest);
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (!result.Succeeded)
            {
                return await Responses.FailurResponse(result.Errors.ToList(), HttpStatusCode.InternalServerError);
            }
            return await Responses.SuccessResponse(user.Id, "Password updated successfully.");
        }

        public async Task<Responses> RemoveUser(AppUser user)
        {
            //await _userManager.DeleteAsync(user);
            var spec = new BaseSpecifications<Property, string>(x => x.OwnerId == user.Id);
            var proprties = await _unitOfWork.Repository<Property, string>().GetAllWithSpecAsync(spec);
            //if (user.ProfileImage != null)
            //{
            //    var list = user.ProfileImage.Split('/');
            //    string imageName = list[list.Length - 1];
            //    await DocumentSettings.DeleteFile(SD.Image, SD.UserProfile, imageName);
            //}
            return await Responses.SuccessResponse(proprties, "User Has Been Deleted Successfully.");
        }
    }
}
