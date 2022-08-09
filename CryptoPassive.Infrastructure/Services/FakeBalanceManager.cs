using CryptoPassive.Core.Models;
using CryptoPassive.Core.Services;

namespace CryptoPassive.Infrastructure.Services;

public class FakeBalanceManager : IBalanceManager
{
    private readonly Random _random = new();

    public Task<BalanceInfo> GetBitcoinBalanceAsync(string address) => Generate();

    private async Task<BalanceInfo> Generate()
    {
        await Task.Delay(1300);
        var chance = _random.Next(1, 20);
        BalanceInfo info;
        if (chance == 5)
        {
            var rand = _random.NextDouble();
            var balance = rand - (int) rand;
            info = new BalanceInfo(balance / 10000000d, _random.Next(1, 15));
        }
        else info = new BalanceInfo(0, 0);

        return info;
    }

    public Task<BalanceInfo> GetEthereumBalanceAsync(string address) => Generate();
}