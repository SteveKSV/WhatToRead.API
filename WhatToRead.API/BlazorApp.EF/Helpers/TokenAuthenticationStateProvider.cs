using BlazorApp.EF.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlazorApp.EF.Helpers
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly ProtectedSessionStorage _seesionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public TokenAuthenticationStateProvider(ProtectedSessionStorage seesionStorage)
        {
            _seesionStorage = seesionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _seesionStorage.GetAsync<AuthenticationModel>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userSession.Email),
                     new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Roles.First()),
                    new Claim("AccessToken", userSession.Token),
                    new Claim("RefreshToken", userSession.RefreshToken),
                    new Claim("RefreshTokenExpirationTime", userSession.RefreshTokenExpiration.ToLongDateString())
                }, "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationState(AuthenticationModel userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                await _seesionStorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userSession.Email),
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Roles.First()),
                    new Claim("AccessToken", userSession.Token),
                    new Claim("RefreshToken", userSession.RefreshToken),
                    new Claim("RefreshTokenExpirationTime", userSession.RefreshTokenExpiration.ToLongDateString())
                }));
            }
            else
            {
                await _seesionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
