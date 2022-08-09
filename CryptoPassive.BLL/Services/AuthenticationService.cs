using CryptoPassive.Core.Enums;
using CryptoPassive.Core.Services;

namespace CryptoPassive.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationManager _authenticationManager;

    public AuthenticationService(IAuthenticationManager authenticationManager)
    {
        _authenticationManager = authenticationManager;
    }

    public bool IsAuthenticated(string token, Cryptocurrency cryptocurrency)
    {
        return cryptocurrency == Cryptocurrency.Bitcoin
            ? !_authenticationManager.IsBitcoinAuth(token)
            : !_authenticationManager.IsEthereumAuth(token);
    }
}