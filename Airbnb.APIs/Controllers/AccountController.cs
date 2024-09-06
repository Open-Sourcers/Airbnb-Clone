using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
namespace Airbnb.APIs.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly IUserService _userService;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IValidator<RegisterDTO> _registerValidator;
        public AccountController(IUserService userService, IValidator<LoginDTO> loginValidator, IValidator<RegisterDTO> registerValidator)
        {
            _userService = userService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
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
    }
}