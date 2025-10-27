using System.Security.Claims;

namespace Node.Domain.Infrastructure.Interfaces.Adapters;
public interface IJwtService
{
    (string, string) CreateJwt(List<Claim> claims);
    ClaimsPrincipal ValidateJwt(string token);
}
