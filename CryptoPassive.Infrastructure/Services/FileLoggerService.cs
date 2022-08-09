using CryptoPassive.Core.Services;

namespace CryptoPassive.Infrastructure.Services;

public class FileLoggerService : IFileLoggerService
{
    public Task Log(string fileName, string text) => File.AppendAllTextAsync(fileName, text + Environment.NewLine);

    public async Task<int> Count(string fileName)
    {
        if (!File.Exists(fileName))
            await File.Create(fileName).DisposeAsync();
        return (await File.ReadAllLinesAsync(fileName)).Length;
    }
}