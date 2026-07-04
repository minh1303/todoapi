using TodoApi.Models;

namespace TodoApi.Services;

public interface IAuthService
{
    public Task<User?> RegisterAsync(string email, string password);
    Task<string?> LoginAsync(string email, string password);
}