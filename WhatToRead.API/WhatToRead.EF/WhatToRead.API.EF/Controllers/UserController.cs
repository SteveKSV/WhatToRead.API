﻿using EFWhatToRead_DAL.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EFWhatToRead_BBL.Models;
using EFTopics.BBL.Data;
using EFTopics.BBL.Entities;

namespace WhatToRead.API.EF.Controllers
{
    /// <summary>
    /// Controller for user logic, such as view users, their roles, roles, jwt-tokens and so on. 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
       
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="context"></param>
        public UserController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        /// <summary>
        /// Method for getting a list of all users that are registered. Only for admin to use.
        /// </summary>
        /// <response code="200">Returns a list of users</response>
        /// <response code="401">Unauthorized - the client provides no credentials or invalid credentials</response>
        /// <response code="403">Forbidden - the client provides valid credentials, but has not enough privileges</response>
        [HttpGet("users")]
        [Authorize(Policy = "OnlyAdmin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IdentityUser>))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Method for getting a user by user's id. Only for admin to use.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>A user</returns>
        /// /// <response code="200">Returns a user by id</response>
        /// <response code="401">Unauthorized - the client provides no credentials or invalid credentials</response>
        /// <response code="403">Forbidden - the client provides valid credentials, but has not enough privileges</response>
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

        /// <summary>
        /// Method for deleting a user by their id. Only for admin to use.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Status Code</returns>
        /// <response code="200">A user is successfully deleted</response>
        /// <response code="401">Unauthorized - the client provides no credentials or invalid credentials</response>
        /// <response code="403">Forbidden - the client provides valid credentials, but has not enough privileges</response>
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

        /// <summary>
        /// Method for creating a user. Only for admin to use.
        /// </summary>
        /// <param name="model">User paramaters</param>
        /// <returns>User</returns>
        /// <response code="200">User is created successfully</response>
        /// <response code="401">Unauthorized - the client provides no credentials or invalid credentials</response>
        /// <response code="403">Forbidden - the client provides valid credentials, but has not enough privileges</response>
        [HttpPost("users")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> CreateUser([FromBody] OtherParamUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Adding role to user
                    var roleResult = await _userManager.AddToRoleAsync(user, model.RoleUser.ToString());

                    if (roleResult.Succeeded)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Method for getting a list of all roles. Only for admin to use.
        /// </summary>
        /// <returns>List of roles</returns>
        [HttpGet("roles")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Method for creating a new role. Only for admin to use.
        /// </summary>
        /// <param name="newRole">Name of the role</param>
        /// <returns>Message, which tells if operation was successfull.</returns>
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

        /// <summary>
        /// Method for deleting a role by it's id. Only for admin to use.
        /// </summary>
        /// <param name="roleId">Role's id</param>
        /// <returns>StatusCode</returns>
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

        /// <summary>
        /// Method for adding a role to user by user's id. Only for admin to use. 
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="model">Model for a role (contains it's name)</param>
        /// <returns>StatusCode</returns>
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

        /// <summary>
        /// Method for removing a role from user by user's id. Only for admin to use.
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="model">Model for a role (contains it's name)</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for getting a refresh tokens that the user have by user's id. Only for admin to use.
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns>List of refresh tokens.</returns>
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
