using Microsoft.AspNetCore.Mvc;
using OAuth2.Domain.Request;
using OAuth2.Service.Contract;
using OAuth2.Service.Exceptions;

namespace OAuth2.WebApi.Controllers
{
    [Route("~/oauth2-api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("assign")]
        public async Task<ActionResult> AssignPermissions([FromBody] RevokeOrAssignPermissionRequest pRequest)
        {
            var response = await _userService.AssignPermissions(pRequest);

            return StatusCode(response.Code, response);
        }

        [HttpPost("revoke")]
        public async Task<ActionResult> RevokePermissions([FromBody] RevokeOrAssignPermissionRequest pRequest)
        {
            try
            {
                await _userService.RevokePermissions(pRequest);
                return NoContent();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
