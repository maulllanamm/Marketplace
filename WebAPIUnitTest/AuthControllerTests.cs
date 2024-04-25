using Marketplace.Controllers;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
    }
}