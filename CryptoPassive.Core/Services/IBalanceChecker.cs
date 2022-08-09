using CryptoPassive.Core.Enums;
using CryptoPassive.Core.Models;

namespace CryptoPassive.Core.Services;

public interface IBalanceChecker
{
    public Task<BalanceInfo> GetBalanceAsync(string address, Cryptocurrency cryptocurrency);
}