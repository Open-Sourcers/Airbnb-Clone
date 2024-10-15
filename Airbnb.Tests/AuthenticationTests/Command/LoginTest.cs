using Airbnb.Application.Services;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Tests.FakeObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;

namespace Airbnb.Tests.AuthenticationTests.Command
{
    public class LoginTest:TestBase
    {
        [Fact]
        public async Task Login_ShouldReturnSuccessResponse_WhenUserLoginSuccessfully()
        {
            //Arrange
            var _userManager=GetUserManager();
            var _signInManager = GetMockSignInManager();
            _signInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            var user = new FakeAccount().Generate();
            user.PasswordHash = "User@123";
            user.EmailConfirmed = true;
            await _userManager.CreateAsync(user);
            
            var loginDto = new LoginDTO
            {
                Email = user.Email,
                Password = user.PasswordHash
            };

            var _authService = new Mock<IAuthService>();
            _authService
                .Setup(x => x.CreateTokenAsync(It.IsAny<AppUser>(), It.IsAny<UserManager<AppUser>>())).ReturnsAsync("Token");

            var handler = new UserService(_userManager, _signInManager.Object, _authService.Object, null, null, null, null);
            // Act
            var result = await handler.Login(loginDto);
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Login_ShouldReturnFailureResponse_WhenUserIsNotFound()
        {
            // Arrange
            var _userManager = GetUserManager();
            var loginDot = new LoginDTO
            {
                Email = "kamal@gmail.com",
                Password = "User@123"
            };

            var handler = new UserService(_userManager, null, null, null, null, null, null);
            // Act
            var result= await handler.Login(loginDot);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task Login_ShouldReturnFailureResponse_WhenPasswordIsNotCorrect()
        {

            //Arrange
            var _userManager = GetUserManager();
            var _signInManager = GetMockSignInManager();
            _signInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            var user = new FakeAccount().Generate();
            user.PasswordHash = "User@123";
            user.EmailConfirmed = true;
            await _userManager.CreateAsync(user);

            var loginDto = new LoginDTO
            {
                Email = user.Email,
                Password = "useR@123"
            };

            var _authService = new Mock<IAuthService>();
            _authService
                .Setup(x => x.CreateTokenAsync(It.IsAny<AppUser>(), It.IsAny<UserManager<AppUser>>())).ReturnsAsync("Token");

            var handler = new UserService(_userManager, _signInManager.Object, _authService.Object, null, null, null, null);
            // Act
            var result = await handler.Login(loginDto);
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Login_ShouldReturnFailureResponse_WhenEmailIsNotConfirmed()
        {

            //Arrange
            var _userManager = GetUserManager();
            var _signInManager = GetMockSignInManager();
            _signInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            var user = new FakeAccount().Generate();
            user.PasswordHash = "User@123";
            await _userManager.CreateAsync(user);

            var loginDto = new LoginDTO
            {
                Email = user.Email,
                Password = "User@123"
            };

            var _authService = new Mock<IAuthService>();
            _authService
                .Setup(x => x.CreateTokenAsync(It.IsAny<AppUser>(), It.IsAny<UserManager<AppUser>>())).ReturnsAsync("Token");

            var handler = new UserService(_userManager, _signInManager.Object, _authService.Object, null, null, null, null);
            // Act
            var result = await handler.Login(loginDto);
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
