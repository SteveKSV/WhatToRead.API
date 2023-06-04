using Identity.Data.Constants;
using Identity.Data.Models;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    /// <summary>
    /// Controller for admin's methods
    /// </summary>
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adminService"></param>
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Method for adding role to concrete user
        /// </summary>
        /// <param name="model">User's email, password and their new role.</param>
        /// <returns>Result of adding.</returns>
        /// <response code="401">Unauthorized</response>
        [HttpPost("AddRoleToUser")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
        {
            var result = await _adminService.AddRoleAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Method of getting all Refresh-Tokens by user's id.
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>Refresh-Tokens list.</returns>
        /// <response code="401">Unauthorized</response>
        [HttpPost("GetRefreshTokens/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public IActionResult GetRefreshTokens(string id)
        {
            var user = _adminService.GetById(id);
            return Ok(user.RefreshTokens);
        }

        /// <summary>
        /// Method for getting all users and their information.
        /// </summary>
        /// <returns>List of users.</returns>
        /// <response code="200">Successfully got users</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">There are not any users</response>
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _adminService.GetUsersAsync();
            if(result == null)
            {
                return NotFound("There aren't any users in system.");
            }
            return Ok(result);
        }

        /// <summary>
        /// Method for getting one user by their id.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>Information about user.</returns>
        /// <response code="200">Successfully got user information</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">There is not any user with this id</response>
        [HttpGet("GetUserById/{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(string userId)
        {
            var result = await _adminService.GetUserByIdAsync(userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Method for deleting user by their id.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Successfully deleted user from database</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">There is not any user with this id</response>
        [HttpDelete("DeleteUser/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _adminService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = _adminService.DeleteUserAsync(user);
            if (!result.Result.Succeeded)
            {
                return BadRequest(result.Result.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Method for creating a user.
        /// </summary>
        /// <param name="model">Registration fields.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Successfully created.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">User wasn't successfully created</response>
        [HttpPost("CreateUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email };

                var result = await _adminService.CreateUserAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var role = new AddRoleModel { Email = user.Email, Password = model.Password, Role = Authorization.default_role.ToString() };

                    var roleResult = await _adminService.AddRoleAsync(role);

                    if (roleResult.Contains("Role not found") || roleResult.Contains("No Accounts Registered with") || roleResult.Contains("Incorrect Credentials for"))
                    {
                        await _adminService.DeleteUserAsync(user);
                        return BadRequest("Role is not assigned");
                    }

                    return Ok($"User {user.UserName} is created successfully and it has a role!");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
