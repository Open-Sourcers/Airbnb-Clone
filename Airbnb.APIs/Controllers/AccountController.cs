using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Airbnb.APIs.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly IUserService _userService;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IValidator<ResetPasswordDTO> _resetPasswordValidator;
        private readonly IValidator<ForgetPasswordDto> _forgetPasswordValidator;
        public AccountController(IUserService userService,
            IValidator<LoginDTO> loginValidator,
            IValidator<RegisterDTO> registerValidator,
            IValidator<ResetPasswordDTO> resetPasswordValidator,
            IValidator<ForgetPasswordDto> forgetPasswordValidator)
        {
            _userService = userService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _forgetPasswordValidator = forgetPasswordValidator;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<Responses>> Login(LoginDTO userDto)
        {
            var validate = await _loginValidator.ValidateAsync(userDto);
            if (!validate.IsValid)
            {
                return await Responses.FailurResponse(validate.Errors);
            }
            return Ok(await _userService.Login(userDto));
        }
        [HttpPost("Register")]
        public async Task<ActionResult<Responses>> Register(RegisterDTO userDto)
        {
            var validate = await _registerValidator.ValidateAsync(userDto);
            if (!validate.IsValid)
            {
                return await Responses.FailurResponse(validate.Errors.ToString());
            }
            return Ok(await _userService.Register(userDto));
        }

        [HttpPost("EmailConfirmation")]
        public async Task<ActionResult<Responses>> EmailConfirmation(string? email, string? code)
        {
            return Ok(await _userService.EmailConfirmation(email, code));
        }

        [HttpGet("ForgetPassword")]
        public async Task<ActionResult<Responses>> ForgetPassword([FromBody]ForgetPasswordDto forgetPassword)
        {
            var validation=await _forgetPasswordValidator.ValidateAsync(forgetPassword);
            if(!validation.IsValid)return await Responses.FailurResponse(validation.Errors.ToString());
            return Ok(await _userService.ForgetPassword(forgetPassword));
        }
        [HttpPut("ResetPassword")]
        public async Task<ActionResult<Responses>> ResetPassword([FromBody]ResetPasswordDTO resetpasssword)
        {
            var validate = await _resetPasswordValidator.ValidateAsync(resetpasssword);
            if (!validate.IsValid)
            {
                return await Responses.FailurResponse(validate.Errors.ToList());
            }

            return Ok(await _userService.ResetPassword(resetpasssword));
        }
    }
}