using Golden_Bekt_Development.Models;

namespace Golden_Bekt_Development.Services
{
    public interface IAdminAuthService
    {
        Task<Admin> RegisterAsync(string username, string password, string name);
        Task<string?> LoginAsync(string username, string password);
    }
}
