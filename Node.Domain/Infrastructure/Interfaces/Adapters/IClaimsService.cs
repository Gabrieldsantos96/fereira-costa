using Node.Domain.Entities;
using Node.Domain.ValueObjects;
using System.Security.Claims;

namespace Node.Domain.Infrastructure.Interfaces.Adapters;
public interface IClaimsService
{
    int? GetUserId();
    Guid GetUserRefId();
    string? GetPhone();
    string? GetStatus();
    public Name GetUsernameValueObject();
    public string GetUsername();
    public Address GetAddressValueObject();
    public string? GetRole();
    bool IsInRole(string role);
    public List<Claim>? GetAllClaims();
    Claim? GetClaimFirstOrDefault(Func<Claim, bool> predicate);
   List<Claim> GenerateClaims(User user);
}
