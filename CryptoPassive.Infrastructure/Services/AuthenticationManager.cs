using CryptoPassive.Core.Services;
using Type = CryptoPassive.Infrastructure.Enums.Type;

namespace CryptoPassive.Infrastructure.Services;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly ApplicationDbContext _applicationDbContext;

    public AuthenticationManager(ApplicationDbContext applicationDbContext) =>
        _applicationDbContext = applicationDbContext;

    
    public bool IsBitcoinAuth(string token)
    {
        var pass= _applicationDbContext.Passwords.FirstOrDefault(pass => pass.PasswordString == token);
        return pass is {Type: Type.Bitcoin or Type.All};
    }

    public bool IsEthereumAuth(string token)
    {
        var pass= _applicationDbContext.Passwords.FirstOrDefault(pass => pass.PasswordString == token);
        return pass is {Type: Type.Ethereum or Type.All};
    }
}