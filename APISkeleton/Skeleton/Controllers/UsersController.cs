using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skeleton.DTO;
using Skeleton.Services.Interfaces;

namespace Skeleton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController
        (
            IUserService userService
        )
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<UserDto> users = await _userService.GetAll();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            var userCreated = _userService.Create(userDto);

            return Ok(userCreated);
        }
    }
}
