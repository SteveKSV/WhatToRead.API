using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WhatToRead.API.EF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtSettings _options;
        private readonly IAccountManager _accountManager;
        public AuthorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JwtSettings> options, IAccountManager accountManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options.Value;
            _accountManager = accountManager;
        }

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
    }
}
