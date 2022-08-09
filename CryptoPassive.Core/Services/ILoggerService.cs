using CryptoPassive.Core.Models;

namespace CryptoPassive.Core.Services;

public interface ILoggerService
{
    public string CreateLogString(Address address, BalanceInfo balance);
    public Task<int> CountAsync();
    public Task LogToFileAsync(string text);
}