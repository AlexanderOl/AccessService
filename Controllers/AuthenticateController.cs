using AccessService.Models;
using AccessService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController(IConfiguration config, AuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost("/")]
        public IActionResult GenerateApiKey([FromBody] AuthenticateRequest userLogin)
        {
            var token = authenticationService.GenerateApiKey(userLogin);
            return Ok($"Your new api key - {token}");
        }

        [HttpPost("/authenticate")]
        public IActionResult Authenticate([FromBody] Guid apiKey)
        {
            var token = authenticationService.GenerateToken(apiKey);
            return token == null ? NotFound() : Ok($"Your JWT token - {token}");
        }
    }
}
