﻿using Airbnb.Domain;
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
        private readonly IValidator<ResetPasswordDTO> _resetPasswordValidator;
        public AccountController(IUserService userService, IValidator<LoginDTO> loginValidator, IValidator<RegisterDTO> registerValidator, IValidator<ResetPasswordDTO> resetPasswordValidator)
        {
            _userService = userService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _resetPasswordValidator = resetPasswordValidator;
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

<<<<<<< HEAD
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<Responses>> ResetPassword(ResetPasswordDTO resetpasssword)
        {
            var validate = await _resetPasswordValidator.ValidateAsync(resetpasssword);
            if (!validate.IsValid)
            {
                return await Responses.FailurResponse(validate.Errors.ToString);
            }
            return Ok(await _userService.ResetPassword(resetpasssword));
=======
        [HttpPost("EmailConfirmation")]
        public async Task<ActionResult<Responses>> EmailConfirmation(string? email, string? code)
        {
            return Ok(await _userService.EmailConfirmation(email, code));
>>>>>>> c49054459ef8de84058b2f4790d3b8bd3c1cc5f7
        }
    }
}