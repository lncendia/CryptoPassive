using CryptoPassive.Core.Enums;
using CryptoPassive.Core.Models;
using CryptoPassive.Core.Services;

namespace CryptoPassive.BLL.Services;

public class BalanceChecker : IBalanceChecker
{
    private readonly IBalanceManager _manager;

    public BalanceChecker(IBalanceManager manager) => _manager = manager;

    public Task<BalanceInfo> GetBalanceAsync(string address, Cryptocurrency cryptocurrency)
    {
        return cryptocurrency == Cryptocurrency.Bitcoin
            ? _manager.GetBitcoinBalanceAsync(address)
            : _manager.GetEthereumBalanceAsync(address);
    }
}