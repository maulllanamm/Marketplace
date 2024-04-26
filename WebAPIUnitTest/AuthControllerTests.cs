using AutoMapper.Internal;
using FluentAssertions;
using Marketplace.Controllers;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Formats.Asn1;
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
            result.Should().BeOfType<OkObjectResult>();
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
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull(); // Assert that the result is not null
            okResult.Should().BeOfType<OkObjectResult>(); // Assert that the result is of type OkObjectResult
            okResult.Value.Should().Be("newAccessToken"); // Assert that the value matches "newAccessToken"
        }

        [Fact]
        public async Task RefreshToken_EmptyRefreshToken_BadRequest()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Cookies["refreshToken"]).Returns(string.Empty);

            // Act
            var result = await _controller.RefreshToken();

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Value.Should().Be("Invalid token.");
            badRequest.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RefreshToken_NullPrincipal_NotFound()
        {
            // Arrange
            var refreshToken = "validRefreshToken";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Cookies["refreshToken"]).Returns(refreshToken);
            _authServiceMock.Setup(m => m.ValidateAccessToken(refreshToken)).Returns((ClaimsPrincipal)null);

            // Act
            var result = await _controller.RefreshToken();

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Value.Should().Be("Invalid token.");
            notFound.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RefreshToken_TokenNotEquals_BadRequest()
        {
            // Arrange
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.Name, "testUser") // Menambahkan klaim langsung
             }));

            var user = new UserViewModel { Username = "testUser", RefreshToken = "valid-token" }; // Asumsikan user ini tidak memiliki refreshToken yang sama dengan yang diberikan

            _userServiceMock.Setup(u => u.GetByUsername(It.IsAny<string>())).ReturnsAsync(user);

            var refreshToken = "refreshToken"; // Misalnya refreshToken ini tidak sama dengan refreshToken dari user yang diberikan
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Cookies["refreshToken"]).Returns(refreshToken);
            _authServiceMock.Setup(s => s.ValidateAccessToken(refreshToken)).Returns(principal);

            // Act
            var result = await _controller.RefreshToken();

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().BeOfType<BadRequestObjectResult>();
            badRequest.Value.Should().Be("Invalid Refresh Token.");
        }

        [Fact]
        public async Task RefreshToken_ExpiredToken_Unauthorized()
        {
            var refreshToken = "refreshToken"; // Misalnya refreshToken ini tidak sama dengan refreshToken dari user yang diberikan
            var user = new UserViewModel
            {
                Username = "testUser",
                RoleName = "user",
                RefreshToken = "refreshToken",
                TokenExpires = DateTime.Now.AddDays(-1) // Mengatur TokenExpires menjadi masa lalu
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testUser"),
                new Claim(ClaimTypes.Role, "user")
            }, "mock"));

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Cookies["refreshToken"]).Returns(refreshToken);
            _authServiceMock.Setup(s => s.ValidateAccessToken(It.IsAny<string>())).Returns(principal);
            _userServiceMock.Setup(u => u.GetByUsername(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _controller.RefreshToken();

            // Assert
            var unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Should().BeOfType<UnauthorizedObjectResult>();
            unauthorized.Value.Should().Be("Token expired.");
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
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

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
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.StatusCode.Should().Be(400);
            badRequest.Value.Should().Be("Username or password did not match.");
        }

        [Fact]
        public async Task Register_SuccessfulRegister_ReturnsOk()
        {
            // Arrange
            var registerRequest = new RegisterViewModal 
            {       
                    RoleId = 1,
                    Username = "user", 
                    Password = "password",
                    Email = "user@gmail.com",
                    FullName = "fulluser",
                    PhoneNumber = "1234567890",
                    Address = "test_address"
            };

            _authServiceMock.Setup(m => m.Register(registerRequest)).ReturnsAsync(new UserViewModel());

            // Act
            var result = await _controller.Register(registerRequest) ;

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }

    }
}