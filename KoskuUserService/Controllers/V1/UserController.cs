using System;
using System.Threading.Tasks;
using KoskuUserService.Contracts;
using KoskuUserService.Managers;
using Microsoft.AspNetCore.Mvc;

namespace KoskuUserService.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        [ProducesResponseType(404, Type = null)]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _userManager.Get(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] UserRequest request)
        {
            var response = await _userManager.Create(request);
            return Created($"api/users/{response.Id}", response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserRequest request)
        {
            var response = await _userManager.Update(id, request);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userManager.Delete(id);
            return Ok();
        }
    }
}
