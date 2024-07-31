using Microsoft.AspNetCore.Mvc;
using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;
using OAuth2.Service.Contract;

namespace OAuth2.WebApi.Controllers
{
    [Route("~/oauth2-api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateRoleRequest pRequest)
        {
            Result<RoleResponses> response = await _roleService.CreateAsync(pRequest);

            return StatusCode(response.Code, response);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateRoleRequest pRequest)
        {
            var response = await _roleService.UpdateAsync(pRequest);

            return StatusCode(response.Code, response);
        }

        [HttpPost("assign")]
        public async Task<ActionResult> AssignRoles([FromBody] AssignRoleRequest pRequest)
        {
            var response = await _roleService.AssignRoles(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
