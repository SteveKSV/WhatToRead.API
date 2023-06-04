using Identity.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.Interfaces
{
    public interface IAdminService
    {
        Task<string> AddRoleAsync(AddRoleModel model);
        User GetById(string id);
        Task<IEnumerable<User>?> GetUsersAsync();
        Task<User?> GetUserByIdAsync(string userId);
        Task<IdentityResult?> DeleteUserAsync(User user);
        Task<IdentityResult?> CreateUserAsync(User user, string password);
    }
}
