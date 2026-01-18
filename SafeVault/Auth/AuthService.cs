using BCrypt.Net;
using SafeVault.Data;

namespace SafeVault.Auth;

public class AuthService
{
    private readonly UserRepository _repo;

    public AuthService(UserRepository repo)
    {
        _repo = repo;
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public (bool ok, string message, string? role) Login(string username, string password)
    {
        var user = _repo.GetByUsername(username);
        if (user == null) return (false, "User not found.", null);

        if (!VerifyPassword(password, user.Value.passwordHash))
            return (false, "Invalid password.", null);

        return (true, "Login successful.", user.Value.role);
    }
}
