using CryptoPassive.Core.Enums;

namespace CryptoPassive.Core.Services;

public interface IAuthenticationService
{
    public bool IsAuthenticated(string token, Cryptocurrency cryptocurrency);
}