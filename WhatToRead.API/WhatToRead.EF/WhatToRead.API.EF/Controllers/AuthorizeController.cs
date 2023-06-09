using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WhatToRead.API.EF.Controllers
{
    /// <summary>
    /// Controller for register, authorization, methods to update password, because user forgot it, and methods for renew and revoke tokens.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtSettings _options;
        private readonly IAccountManager _accountManager;

        /// <param name="userManager">Manager for instruments with user.</param>
        /// <param name="signInManager">Provides an api for user to sign in</param>
        /// <param name="options"></param>
        /// <param name="accountManager"></param>
        public AuthorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JwtSettings> options, IAccountManager accountManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options.Value;
            _accountManager = accountManager;
        }

        /// <summary>
        /// Register a user.
        /// </summary>
        /// <param name="paramUser">Email, password, username and role (admin or simple user)</param>
        /// <returns></returns>
        /// <response code="200">User is successfully registered</response>
        /// <response code="400">User registration was not successfull</response>
        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(OtherParamUser paramUser)
        {
            var user = new IdentityUser { UserName = paramUser.UserName, Email = paramUser.Email };

            var result = await _userManager.CreateAsync(user, paramUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim("RoleUser", paramUser.RoleUser.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, paramUser.Email));

                await _userManager.AddClaimsAsync(user, claims);
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="paramUser">Email, password</param>
        /// <returns>Token model (access token and refresh token)</returns>
        /// <response code="200">User is successfully loged in</response>
        /// <response code="400">User logging was not successfull</response>
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginModel paramUser)
        {
            var token = await _accountManager.GetAuthTokens(paramUser);

            if (token != null)
            {
                return Ok(token);
            }

            return BadRequest();
        }

        /// <summary>
        /// Gives us a token for reseting a password.
        /// </summary>
        /// <param name="model">Email for getting token for reset password for this user</param>
        /// <returns>Reset token for reset password</returns>
        /// <response code="200">This user is exist and we get a token</response>
        /// <response code="400">This user isn't exist and we don't get a token</response>
        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

                // TODO: Send the reset link to the user's email

                return Ok(token);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Reset a password for user a user.
        /// </summary>
        /// <param name="resetPassword">Email for reseting a password, token for reset password, which we get from forgot-password method,
        /// new password</param>
        /// <returns>Status code</returns>
        /// <response code="200">The password is successfully reseted</response>
        /// <response code="400">The password isn't successfully reseted</response>
        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return BadRequest("Invalid email address");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Method for renew tokens.
        /// </summary>
        /// <param name="refreshToken">Token for renewing</param>
        /// <returns>A new tokens</returns>
        /// <response code="200">The operation of renewing token was successfull</response>
        /// <response code="400">The operation of renewing token wasn't successfull</response>
        [HttpPost]
        [Route("Renew-Token")]
        public async Task<IActionResult> RenewTokens(RenewTokenDto refreshToken)
        {
            var tokens = await _accountManager.RenewTokens(refreshToken);
            if (tokens == null)
            {
                return ValidationProblem("Invalid Refresh Token");
            }
            return Ok(tokens);
        }

        /// <summary>
        /// Method for revoke tokens.
        /// </summary>
        /// <param name="request">Token for revokeing</param>
        /// <returns>Status code</returns>
        /// <response code="200">The token was successfully revoked</response>
        /// <response code="400">The token wasn't successfully revoked</response>
        [HttpPost("Revoke-Token")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeTokenRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            await _accountManager.RevokeTokens(request);

            return Ok();
        }

    }
}
