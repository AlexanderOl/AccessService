using AccessService.Models.Enums;

namespace AccessService.Models;
public class ApiTokenDetails(AuthenticateRequest userLogin)
{
    public Guid Token { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userLogin.UserId;
    public ApiTokenStatus Status { get; set; } = ApiTokenStatus.Active;
    public DateTime LastUsage { get; set; } = DateTime.Now;
    public List<UserPermission> Permissions { get; set; } = userLogin.Permissions;
}
