using AdminDashboard.Abstractions.Common;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.Auth;

public interface IUsersRepository : IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> VerifyCredentialsAsync(string email, string password);
}