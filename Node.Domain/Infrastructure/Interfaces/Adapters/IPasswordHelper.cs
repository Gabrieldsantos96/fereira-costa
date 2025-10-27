using Node.Domain.Entities;

namespace Node.Domain.Infrastructure.Interfaces.Adapters;
public interface IPasswordHelper
{
    string GeneratePassword(User user, string password);
    bool VerifyPassword(User user, string hashedPassword, string password);
}