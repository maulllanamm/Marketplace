using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }


        [HttpPut]
        public async Task<ActionResult> Put(UserViewModel request)
        {
            var user = await _userService.Update(request);
            return Ok(user);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            var user =  await _userService.Delete(id);
            return Ok(user);
        }
    }
}