using CryptoPassive.Core.Models;

namespace CryptoPassive.Core.Services;

public interface IBalanceManager
{
    public Task<BalanceInfo> GetBitcoinBalanceAsync(string address);
    public Task<BalanceInfo> GetEthereumBalanceAsync(string address);
}