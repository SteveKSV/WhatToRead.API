using BlazorApp.EF.Models;

namespace BlazorApp.EF.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterAsync(RegisterModel model)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.PostAsJsonAsync($"{url}/User/Register", model);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
        {
            var authenticationModel = new AuthenticationModel();

            // Make a request to the API endpoint to perform login and receive the cookie token
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.PostAsJsonAsync($"{url}/User/Login", model);
            response.EnsureSuccessStatusCode();

            // Read the authentication model from the response
            //var result = await response.Content.ReadFromJsonAsync<AuthenticationModel>();
            return await response.Content.ReadFromJsonAsync<AuthenticationModel>();
            //if (result != null && result.IsAuthenticated)
            //{
            //    // Update the authentication model with the necessary data
            //    authenticationModel.IsAuthenticated = true;
            //    authenticationModel.UserName = result.UserName;
            //    authenticationModel.Email = result.Email;
            //    authenticationModel.RefreshToken = result.RefreshToken;
            //    authenticationModel.RefreshTokenExpiration = result.RefreshTokenExpiration;
            //    authenticationModel.Token = result.Token;
            //}
            //else
            //{
            //    authenticationModel.IsAuthenticated = false;
            //    authenticationModel.Message = result.Message;
            //}

            //return authenticationModel;
        }

        public async Task<AuthenticationModel> RefreshTokenAsync()
        {
            var authenticationModel = new AuthenticationModel();

            // Make a request to the API endpoint to refresh the cookie token
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.PostAsync($"{url}/User/Refresh-Token", null);
            response.EnsureSuccessStatusCode();

            // Read the authentication model from the response
            var result = await response.Content.ReadFromJsonAsync<AuthenticationModel>();

            if (result != null && result.IsAuthenticated)
            {
                // Save the refreshed cookie token received from the response
                var token = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                // Update the authentication model with the necessary data
                authenticationModel.IsAuthenticated = true;
                authenticationModel.Token = token; // Store the token for future API requests or to pass to other services
                authenticationModel.Email = result.Email;
                authenticationModel.UserName = result.UserName;
                authenticationModel.Roles = result.Roles;
                authenticationModel.RefreshToken = result.RefreshToken;
                authenticationModel.RefreshTokenExpiration = result.RefreshTokenExpiration;
            }
            else
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = result.Message;
            }

            return authenticationModel;
        }
    }
}
