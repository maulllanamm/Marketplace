using Marketplace.Controllers;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace WebAPIUnitTest
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task GetMe_ReturnOkObjectResult()
        {
            var authServiceMock = new Mock<IAuthService>();
            var userServiceMock = new Mock<IUserService>();
            var controller = new AuthController(authServiceMock.Object, userServiceMock.Object);

            var result = await controller.GetMe();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_SuccessfulLogin_ReturnsOk()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var userServiceMock = new Mock<IUserService>();

            var loginRequest = new LoginViewModal { Username = "test_user", Password = "password123" };
            var user = new UserViewModel { Username = "test_user", RoleName = "Admin" };

            authServiceMock.Setup(m => m.Login(loginRequest)).ReturnsAsync(user);
            authServiceMock.Setup(m => m.GenerateAccessToken(user.Username, user.RoleName)).ReturnsAsync("fake_access_token");
            authServiceMock.Setup(m => m.GenerateRefreshToken(user.Username)).ReturnsAsync("fake_refresh_token");

            var controller = new AuthController(authServiceMock.Object, userServiceMock.Object);

            // Act
            var result = await controller.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Login_UnsuccessfulLogin_ReturnsBadRequest()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var userServiceMock = new Mock<IUserService>();

            var loginRequest = new LoginViewModal { Username = "invalid_user", Password = "invalid_password" };

            authServiceMock.Setup(m => m.Login(loginRequest)).ReturnsAsync((UserViewModel)null);

            var controller = new AuthController(authServiceMock.Object, userServiceMock.Object);

            // Act
            var result = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Username or password did not match.", result.Value);
        }

    }
}