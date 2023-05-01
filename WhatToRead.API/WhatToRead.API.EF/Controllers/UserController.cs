using EFWhatToRead_DAL.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EFWhatToRead_BBL.Models;
using EFTopics.DAL.Data;

namespace WhatToRead.API.EF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _options;
        private readonly ApplicationContext _context;
        public UserController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IOptions<JwtSettings> options,
                              ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options.Value;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("users")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("users/{userId}")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("users/{userId}")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("users")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> CreateUser([FromBody] OtherParamUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }


        [HttpGet("roles")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost("roles")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> CreateRoles(string? newRole)
        {
            // Створюємо ролі, якщо вони ще не створені
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (!newRole.IsNullOrEmpty() && !await _roleManager.RoleExistsAsync(newRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(newRole));
                return Ok($"{newRole} is successfully craeted");
            }

            return BadRequest($"Role is not created");
        }

        [HttpDelete("roles/{roleId}")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("users/{userId}/roles")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> AddRoleToUser(string userId, [FromBody] AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpDelete("users/{userId}/roles")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, [FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("users/{userId}/refresh-tokens")]
        [Authorize(Policy = "OnlyAdmin")]

        public async Task<IActionResult> GetUserRefreshTokensById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User is not found");
            }

            var refreshTokens = await _context.RefreshTokens.Where(u=>u.UserId == userId).ToListAsync();

            if (refreshTokens.IsNullOrEmpty())
            {
                return NotFound($"There are not refresh tokens for user with id {userId}");
            }

            return Ok(refreshTokens);
        }
    }

}
