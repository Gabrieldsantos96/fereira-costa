using Node.Domain.Entities;

namespace Node.Domain.Infrastructure.Interfaces.Adapters;
public interface IAuthenticationService
{
    Task<User?> GetUserAsync(string email, CancellationToken ct);
    Task CreateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken ct);
    Task DeleteRefreshTokenAsync(string hash, CancellationToken ct);
    Task<RefreshToken> ValidateRefreshTokenAsync(string hash, CancellationToken ct);
    Task RenewRefreshTokenAsync(RefreshToken current, RefreshToken newToken, CancellationToken ct);
}
