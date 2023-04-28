using AutoMapper;
using EFTopics.DAL.Data;
using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Models;
using EFWhatToRead_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EFWhatToRead_BBL.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationContext _context;
        private IMapper Mapper { get; }
        public AccountManager(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JwtSettings> jwtSettings, ApplicationContext applicationContext, IMapper mapper)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _context = applicationContext;
            Mapper = mapper;
        }

        private string GetToken(IEnumerable<Claim> principal)
        {
            var claims = principal.ToList();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var jwt = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(1),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshtoken = Convert.ToBase64String(tokenBytes);

            var tokenIsInUser = _context.RefreshTokens.Any(_ => _.Token == refreshtoken);

            if (tokenIsInUser)
            {
                return CreateRefreshToken();
            }
            return refreshtoken;
        }
        public async Task<TokenDto?> GetAuthTokens(LoginModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);

            if (result.Succeeded)
            {
                IEnumerable<Claim> claims = await _userManager.GetClaimsAsync(user);
                var accessToken = GetToken(claims);
                var refreshToken = CreateRefreshToken();
                await InsertRefreshToken(user.Id, refreshToken);
                return new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return null;
            }
        }

        private async Task InsertRefreshToken(string userId, string refreshtoken)
        {
            var newRefreshTokenDto = new RefreshTokenDto
            {
                UserId = userId,
                Token = refreshtoken,
                ExpirationDate = DateTime.Now.AddDays(7)
            };

            var newRefreshToken = Mapper.Map<RefreshTokenDto, RefreshToken>(newRefreshTokenDto);
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();
        }
        public async Task<TokenDto?> RenewTokens(RenewTokenDto refreshToken)
        {
            var userRefreshToken = _context.RefreshTokens
                                     .Where(_ => _.Token == refreshToken.Token
                                                && _.ExpirationDate >= DateTime.Now)
                                     .FirstOrDefault();

            if (userRefreshToken == null)
            {
                return null;
            }

            var user =  _userManager.Users.Where(_ => _.Id == userRefreshToken.UserId).FirstOrDefault();

            if (user == null )
            {
                return null;
            }

            IEnumerable<Claim> claims = await _userManager.GetClaimsAsync(user);

            var newJwtToken = GetToken(claims);
            var newRefreshToken = CreateRefreshToken();

            userRefreshToken.Token = newRefreshToken;
            userRefreshToken.ExpirationDate = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync();

            return new TokenDto
            {
                AccessToken = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
