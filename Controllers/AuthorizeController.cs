using AccessService.Constants;
using AccessService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccessService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizeController(AuthorizeService authorizeService) : BaseController
    {

        [HttpGet("/")]
        [Authorize(Policy = AuthConstants.RequireReadPermission)]
        public IActionResult GetAllTokens()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var allTokenDetails = authorizeService.GetAllTokens(userId.Value);
            return Ok(allTokenDetails);
        }

        [HttpDelete("/{apiToken:guid}")]
        [Authorize(Policy = AuthConstants.RequireWritePermission)]
        public IActionResult RevokeToken(Guid apiToken)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var success = authorizeService.RevokeApiToken(userId.Value, apiToken);
            return Ok(success);
        }

    }
}
