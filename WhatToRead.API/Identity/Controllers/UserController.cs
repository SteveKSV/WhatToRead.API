using FluentValidation;
using Identity.Data.Models;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    /// <summary>
    /// Contoller for Registration, SingIn, Reset Password, Get New and Revoke Refresh Token for User.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<RegisterModel> _validatorForSignUp;
        private readonly IValidator<TokenRequestModel> _validatorForSignIn;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userService"></param>
        /// <param name="_validatorForSignUp"></param>
        /// <param name="_validatorForSignIn"></param>
        public UserController(UserManager<User> userManager, IUserService userService, IValidator<RegisterModel> _validatorForSignUp, IValidator<TokenRequestModel> _validatorForSignIn)
        {
            _userService = userService;
            _userManager = userManager;
            this._validatorForSignUp = _validatorForSignUp;
            this._validatorForSignIn = _validatorForSignIn;
        }

        /// <summary>
        /// Method for registration.
        /// </summary>
        /// <param name="model">Register model (First and Last names, Username, Email and Password)</param>
        /// <returns>Returns string with information about registration (is it successfull or not and why)</returns>
        /// <response code="200">Validated</response>
        /// <response code="400">Failed validation</response>
        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RegisterAsync(RegisterModel model)
        {
            var validationResult = _validatorForSignUp.Validate(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.RegisterAsync(model);
                return Ok(result);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }

        }

        /// <summary>
        /// Method for login.
        /// </summary>
        /// <param name="model">Email and password for SignIn</param>
        /// <returns>Authentication model, which consists of message about errors, if there are any, status of authentication, username, email, access token and roles.</returns>
        /// <response code="200">Validated</response>
        /// <response code="400">Failed validation, there isn't this user, Refresh-Token error</response>
        /// <response code="404">There isn't this user</response>
        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> LoginAsync(TokenRequestModel model)
        {
            var validationResult = _validatorForSignIn.Validate(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.GetTokenAsync(model);

                if (result == null)
                {
                    return NotFound(result);
                }

                if (string.IsNullOrEmpty(result.RefreshToken))
                {
                    // Handle null or empty refreshToken value
                    return BadRequest("Refresh token is missing or invalid.");
                }

                SetRefreshTokenInCookie(result.RefreshToken);
                return Ok(result);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }

        /// <summary>
        /// ForgotPassword method, which returns token for resetting an old passoword.
        /// </summary>
        /// <param name="model">Email</param>
        /// <returns>Token for resetiing password</returns>

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

                return Ok(token);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Method for resetting password.
        /// </summary>
        /// <param name="resetPassword">Email, token from ForgotPassword end-point, new password.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Successfully changed password</response>
        /// <response code="400">Password wasn't changed</response>
        /// <response code="500">Server error</response>
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
        /// Method for refreshing Access-Token. This method gets from cookies Refresh-Token and uses it to refresh Access-Token, then it updates Refresh-Token in Cookies.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshTokenAsync(refreshToken);
            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }

        /// <summary>
        /// Method for revoking Refresh-Tokens. It has parametr Refresh-Token or it can use Cookie Refresh-Token
        /// </summary>
        /// <param name="model">Refresh-Token</param>
        /// <returns>Message that tells if revoking was successfull.</returns>
        /// <response code="200">Successfully revoked</response>
        /// <response code="404">Token wasn't found</response>
        /// <response code="400">Token is requied</response>
        [HttpPost("Revoke-Token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });
            var response = _userService.RevokeToken(token);
            if (!response)
                return NotFound(new { message = "Token not found" });
            return Ok(new { message = "Token revoked" });
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
