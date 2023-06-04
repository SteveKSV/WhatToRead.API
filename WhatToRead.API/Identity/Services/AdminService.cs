using Identity.Data.Models;
using Identity.Data;
using Identity.Services.Interfaces;
using Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Identity.Data.Constants;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services
{
    /// <summary>
    /// Class for business logic for admin methods: add role to user, create new user itself, etc.
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSetting _jwt;
        private readonly JwtDbContext _jwtDbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="jwt"></param>
        /// <param name="jwtDbContext"></param>
        public AdminService(UserManager<User> userManager, IOptions<JwtSetting> jwt, JwtDbContext jwtDbContext)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _jwtDbContext = jwtDbContext;
        }

        /// <summary>
        /// Method for adding role to concrete user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>String message of success or failure</returns>
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return $"No Accounts Registered with {model.Email}.";
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roleExists = Enum.GetNames(typeof(Authorization.Roles)).Any(x => x.ToLower() == model.Role.ToLower());
                if (roleExists)
                {
                    var validRole = Enum.GetValues(typeof(Authorization.Roles)).Cast<Authorization.Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
                    await _userManager.AddToRoleAsync(user, validRole.ToString());
                    return $"Added {model.Role} to user {model.Email}.";
                }
                return $"Role {model.Role} not found.";
            }
            return $"Incorrect Credentials for user {user.Email}.";
        }

        /// <summary>
        /// Method for finding one user by their id.
        /// </summary>
        /// <param name="id">Find user by id</param>
        /// <returns></returns>
        public User GetById(string id)
        {
            return _jwtDbContext.Users.Find(id);
        }

        /// <summary>
        /// Method for getting list of users.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>?> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            if(users != null)
                return users;
            return null; 
        }

        /// <summary>
        /// Method for finding user by their id in Identity table.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user; 
        }

        /// <summary>
        /// Method for deleting user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult?> DeleteUserAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result;
        }
        
        /// <summary>
        /// Method for creating user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult?> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }
    }
}
