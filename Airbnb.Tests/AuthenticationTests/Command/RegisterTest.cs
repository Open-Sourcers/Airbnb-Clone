using Airbnb.Application.Services;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Tests.FakeObjects;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using System.Net;

namespace Airbnb.Tests.AuthenticationTests.Command
{
    public class RegisterTest : TestBase
    {
        [Fact]
        public async Task Register_ShouldBeReturnSuccessResponse_WhenRegistrationDoneSuccessfully()
        {
            //Arrange
            var _userManager = GetUserManager();
            var _signInManager = GetSignInManager();

            var _authService = new Mock<IAuthService>();
            _authService
                .Setup(x => x.CreateTokenAsync(It.IsAny<AppUser>(), It.IsAny<UserManager<AppUser>>()))
                .ReturnsAsync("Token");
            var _mailService = new Mock<IMailService>();
            _mailService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<IFormFile>>()))
                .Returns(Task.CompletedTask);

            var _cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            var _mapper = _serviceProvider.GetRequiredService<IMapper>();
            var _unitOfWork = GetUnitOfWork();

            var user = new FakeRegisterDto().Generate();
            user.Password = "User@123";
            user.roles = new List<Role> { Role.Customer };
            var handler = new UserService(
                _userManager,
                _signInManager,
                _authService.Object,
                _mailService.Object,
                _mapper,
                _cache,
                _unitOfWork
                );
            // Act
            var result = await handler.Register(user);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        [Fact]
        public async Task Register_ShouldReturnFailureResponse_WhenUserNotHaveRoles()
        {
            //Arrange
            var _userManager = GetUserManager();
            var _signInManager = GetSignInManager();

            var _authService = new Mock<IAuthService>();
            _authService
                .Setup(x => x.CreateTokenAsync(It.IsAny<AppUser>(), It.IsAny<UserManager<AppUser>>()))
                .ReturnsAsync("Token");

            var _mailService = new Mock<IMailService>();
            _mailService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<IFormFile>>()))
                .Returns(Task.CompletedTask);

            var _cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            var _mapper = _serviceProvider.GetRequiredService<IMapper>();
            var _unitOfWork = GetUnitOfWork();

            var user = new FakeRegisterDto().Generate();
            user.Password = "User@123";

            var handler = new UserService(
                _userManager,
                _signInManager,
                _authService.Object,
                _mailService.Object,
                _mapper,
                _cache,
                _unitOfWork
                );
            // Act
            var result = await handler.Register(user);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        }
        [Fact]
        public async Task Register_ShouldReturnFailureResponse_WhenUserIsAlreadyExist()
        {
            //Arrange
            var _userManager = GetUserManager();
            var user = new FakeAccount().Generate();
            var userToRegister = new FakeRegisterDto().Generate();
            userToRegister.Email = user.Email;
            await _userManager.CreateAsync(user);

            var handler = new UserService(_userManager, null, null, null, null, null, null);
            //Act
            var result = await handler.Register(userToRegister);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Register_ShouldReturnFailureResponse_WhenUserPasswordIsNotMachPasswordRoles()
        {
            //Arrange
            var _userManager = GetUserManager();
            var user = new FakeRegisterDto().Generate();

            var handler = new UserService(_userManager, null, null, null, null, null, null);
            //Act
            var result = await handler.Register(user);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
        [Fact]
        public async Task Register_ShouldReturnFailureResponse_WhenRolesIsNotCorrect()
        {

            //Arrange
            var _userManager = GetUserManager();
            var user = new FakeRegisterDto().Generate();
            user.Password = "User@123";
            user.roles = new List<Role>() { Role.Other };// Role is not Found in data base
            var handler = new UserService(_userManager, null, null, null, null, null, null);
            //Act
            var result = await handler.Register(user);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        }

    }
}
