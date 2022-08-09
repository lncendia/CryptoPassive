namespace CryptoPassive.Core.Services;

public interface IFileLoggerService
{
    public Task Log(string fileName, string text);
    public Task<int> Count(string fileName);
}