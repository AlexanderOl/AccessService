using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccessService.Controllers
{
    public class BaseController : ControllerBase
    {

        protected Guid? GetUserId()
        {
            var user = HttpContext.User;

            if (!user.Identity.IsAuthenticated || user is not ClaimsPrincipal principal) return null;
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return null;
            var userId = userIdClaim.Value;
            return Guid.Parse(userId);
        }
    }
}
