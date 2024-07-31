using Microsoft.AspNetCore.Mvc;
using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;
using OAuth2.Service.Contract;

namespace OAuth2.WebApi.Controllers
{
    [Route("~/oauth2-api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            Result<RegistrationResponses> response = await _accountService.RegisterAsync(request);

            return StatusCode(response.Code, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Result<LoginResponses> response = await _accountService.LoginAsync(request);

            return StatusCode(response.Code, response);
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginRequest request)
        {
            Result<LoginResponses> response = await _accountService.GoogleLoginAsync(request);

            return StatusCode(response.Code, response);
        }
    }
}
