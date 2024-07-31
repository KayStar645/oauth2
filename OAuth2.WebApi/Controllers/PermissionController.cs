using Microsoft.AspNetCore.Mvc;
using OAuth2.Domain.Request;
using OAuth2.Service.Contract;

namespace OAuth2.WebApi.Controllers
{
    [Route("~/oauth2-api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService pPermissionService)
        {
            _permissionService = pPermissionService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await _permissionService.Get();

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] List<PermissionRequest> pRequest)
        {
            var response = await _permissionService.Create(pRequest);

            return StatusCode(response.Code, response);
        }


    }
}
