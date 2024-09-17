using Airbnb.Application.Services;
using Airbnb.Application.Settings;
using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Airbnb.APIs.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UsersController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly IValidator<UpdateUserDTO> _updateUserValidator;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IMapper _mapper;
        public UsersController(UserManager<AppUser> userManager, IUserService userService, IValidator<UpdateUserDTO> updateUserValidator, IValidator<RegisterDTO> registerValidator, IMapper mapper)
        {
            _userManager = userManager;
            _userService = userService;
            _updateUserValidator = updateUserValidator;
            _registerValidator = registerValidator;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<Responses>> GetAllUsers()
        {
            var AllUsers = await _userManager.Users.ToListAsync();
            return Ok(await Responses.SuccessResponse(_mapper.Map<IEnumerable<UserDTO>>(AllUsers)));
        }
        [HttpGet("GetUserById")]
        public async Task<ActionResult<Responses>> GetUserById(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return Ok(await Responses.FailurResponse("User Not Found", HttpStatusCode.NotFound));
            }
            return Ok(await Responses.SuccessResponse(_mapper.Map<UserDTO>(user)));
        }

        [HttpDelete("RemoveUser")]
        public async Task<ActionResult<Responses>> RemoveUser(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return Ok(await Responses.FailurResponse("InValid User Id", HttpStatusCode.NotFound));
            }
            await _userManager.DeleteAsync(user);
            if (user.ProfileImage != null)
            {
                var list = user.ProfileImage.Split('/');
                string imageName = list[list.Length - 1];
                await DocumentSettings.DeleteFile(SD.Image,SD.UserProfile,imageName);
            }
            return Ok(await Responses.SuccessResponse("User Has Been Deleted Successfully."));
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<Responses>> CreateUser(RegisterDTO userDto)
        {
            var validate = await _registerValidator.ValidateAsync(userDto);
            if (!validate.IsValid)
            {
                return await Responses.FailurResponse(validate.Errors);
            }
            var user = _userManager.FindByEmailAsync(userDto.Email);
            if (user is not null) return await Responses.FailurResponse("Email Is Already Exist!.");
            return Ok(await _userService.CreateUserAsync(userDto));
        }
        [HttpPut("UpdateUser")]
        public async Task<ActionResult<Responses>> UpdateUser(string Id, [FromBody] UpdateUserDTO userDto)
        {
            var validate = await _updateUserValidator.ValidateAsync(userDto);
            if (!validate.IsValid)
            {
                return await Responses.FailurResponse(validate.Errors);
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return await Responses.FailurResponse("User Not Found", HttpStatusCode.NotFound);
            }

            return Ok(await _userService.UpdateUser(user, userDto));
        }
    }
}
