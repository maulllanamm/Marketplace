using Marketplace.Controllers;
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

        private readonly AuthController _controller;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userServiceMock = new Mock<IUserService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _controller = new AuthController(_authServiceMock.Object, _userServiceMock.Object, _httpContextAccessorMock.Object);
        }


        [Fact]
        public async Task GetMe_ReturnOkObjectResult()
        {
            var result = await _controller.GetMe();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_SuccessfulCreateRefreshToken_ReturnOk()
        {
            // Arrange
            var refreshToken = "validRefreshToken";
            var user = new UserViewModel
            {
                Username = "testUser",
                RoleName = "user",
                RefreshToken = "validRefreshToken",
                TokenExpires = DateTime.Now.AddDays(1)
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Cookies["refreshToken"]).Returns(refreshToken);
            _authServiceMock.Setup(x => x.ValidateAccessToken(refreshToken)).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "testUser") })));
            _userServiceMock.Setup(x => x.GetByUsername("testUser")).ReturnsAsync(user);
            _authServiceMock.Setup(x => x.GenerateAccessToken("testUser", "user")).ReturnsAsync("newAccessToken");
            _authServiceMock.Setup(x => x.GenerateRefreshToken("testUser")).ReturnsAsync("newRefreshToken");

            // Act
            var result = await _controller.RefreshToken();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("newAccessToken", okResult.Value);
        }

        [Fact]
        public async Task Login_SuccessfulLogin_ReturnsOk()
        {
            // Arrange
            var loginRequest = new LoginViewModal { Username = "test_user", Password = "password123" };
            var user = new UserViewModel { Username = "test_user", RoleName = "Admin" };

            _authServiceMock.Setup(m => m.Login(loginRequest)).ReturnsAsync(user);
            _authServiceMock.Setup(m => m.GenerateAccessToken(user.Username, user.RoleName)).ReturnsAsync("fake_access_token");
            _authServiceMock.Setup(m => m.GenerateRefreshToken(user.Username)).ReturnsAsync("fake_refresh_token");

            // Act
            var result = await _controller.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Login_UnsuccessfulLogin_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new LoginViewModal { Username = "invalid_user", Password = "invalid_password" };
            _authServiceMock.Setup(m => m.Login(loginRequest)).ReturnsAsync((UserViewModel)null);

            // Act
            var result = await _controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Username or password did not match.", result.Value);
        }

    }
}