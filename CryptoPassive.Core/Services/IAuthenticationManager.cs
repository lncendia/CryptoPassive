namespace CryptoPassive.Core.Services;

public interface IAuthenticationManager
{
    public bool IsBitcoinAuth(string token);
    public bool IsEthereumAuth(string token);
}