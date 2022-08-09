using CryptoPassive.Core.Enums;
using CryptoPassive.Core.Models;
using CryptoPassive.Core.Services;

namespace CryptoPassive.BLL.Services;

public class LoggerService : ILoggerService
{
    private readonly IFileLoggerService _loggerService;
    private readonly string _fileName;

    public LoggerService(IFileLoggerService loggerService, string fileName)
    {
        _loggerService = loggerService;
        _fileName = fileName;
    }

    public string CreateLogString(Address address, BalanceInfo balance)
    {
        var key = address.Type == Cryptocurrency.Bitcoin
            ? $"| Key: {address.PrivateKey,52} |"
            : $"| Key: {address.PrivateKey,64} |";
        return key +
               $" Address: {address.PublicAddress,-42} | Balance: {$"{balance.Balance:f16} {address.Type.ToString()}",-28} | Transactions: {balance.TransactionCount,-6} |";
    }

    public Task LogToFileAsync(string text) => _loggerService.Log(_fileName, text);

    public Task<int> CountAsync() => _loggerService.Count(_fileName);
}