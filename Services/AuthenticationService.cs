using AccessService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccessService.Constants;
using AccessService.Models.Enums;

namespace AccessService.Services;
public class AuthenticationService(CacheService cacheService, IConfiguration config)
{
    public Guid GenerateApiKey(AuthenticateRequest userLogin)
    {
        var apiToken = new ApiTokenDetails(userLogin);
        cacheService.AddApiKey(userLogin.UserId, apiToken);

        return apiToken.Token;
    }

    public string? GenerateToken(Guid apiKey)
    {
        var apiTokenDetails = cacheService.GetApiTokenDetails(apiKey);
        if (apiTokenDetails == null) return null;

        if(apiTokenDetails.Status != ApiTokenStatus.Active) return null;    

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, apiTokenDetails.UserId.ToString())
        };

        apiTokenDetails.Permissions.ForEach(f=> claims.Add(new Claim(AuthConstants.Permission, f.ToString())));

        var token = new JwtSecurityToken(config["Jwt:Issuer"],
            config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

