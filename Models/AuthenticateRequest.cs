using AccessService.Models.Enums;

namespace AccessService.Models;
public class AuthenticateRequest
{
    public required Guid UserId { get; set; }
    public required List<UserPermission> Permissions { get; set; }
}
