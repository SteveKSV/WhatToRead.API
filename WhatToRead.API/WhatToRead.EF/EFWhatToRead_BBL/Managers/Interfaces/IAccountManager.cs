using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_BBL.Models;

namespace EFWhatToRead_BBL.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<TokenDto?> GetAuthTokens(LoginModel login);
        Task<TokenDto?> RenewTokens(RenewTokenDto refreshToken);
        Task<RevokeTokenResponseDto?> RevokeTokens(RevokeTokenRequestDto revokeToken);
    }
}
