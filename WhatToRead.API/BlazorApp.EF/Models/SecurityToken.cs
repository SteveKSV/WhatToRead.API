namespace BlazorApp.EF.Models
{
    public class SecurityToken
    {
        public SecurityToken(string accessToken, string refreshToken, DateTime refreshExpirationDate)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshExpirationDate = refreshExpirationDate;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpirationDate { get; set; }
    }
}
