using CryptoPassive.Core.Enums;
using CryptoPassive.Core.Models;
using CryptoPassive.Core.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CryptoPassive;

public class Application
{
    private readonly IBalanceChecker _balanceChecker;
    private readonly IMnemonicService _mnemonicService;
    private readonly IAuthenticationService _authService;
    private readonly ILoggerService _loggerService;
    private readonly string _fileName;
    private readonly string _logoName;

    public Application(IAuthenticationService authService,
        ILoggerService loggerService, IBalanceChecker balanceChecker, IMnemonicService mnemonicService, string fileName,
        string logoName)
    {
        _authService = authService;
        _loggerService = loggerService;
        _balanceChecker = balanceChecker;
        _mnemonicService = mnemonicService;
        _fileName = fileName;
        _logoName = logoName;
    }

    public async Task Run()
    {
        Console.Title = "CryptoPassive | Крипту на пассив с Павлом";
        Console.WriteLine("Выьерите криптовалюту для поиска:\n1 - BTC\n2 - ETH");
        var success = int.TryParse(Console.ReadLine(), out var currency);
        if (!success || currency is < 1 or > 2)
        {
            Console.WriteLine("Неверный ввод.");
            Console.ReadKey();
            return;
        }

        var currencyType = (Cryptocurrency) (currency - 1);
        Console.WriteLine("Вставьте ваш токен:");
        var token = Console.ReadLine() ?? string.Empty;
        if (_authService.IsAuthenticated(token, currencyType))
        {
            Console.WriteLine("Токен недействителен.");
            Console.ReadKey();
            return;
        }

        await Greeting();
        Console.WriteLine("Введите количество дериваций для мнемонической фразы:");
        success = int.TryParse(Console.ReadLine(), out var count);
        if (!success)
        {
            Console.WriteLine("Неверный ввод.");
            Console.ReadKey();
            return;
        }

        await Start(count, currencyType);
    }

    private async Task Greeting()
    {
        using var image = Image.Load<Rgba32>(_logoName);
        ConsoleWriteImage(image);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            $"Добро пожаловать в CryptoPassive\nTG: @Crypto_Passive1\nНайдено кошельков: {(await _loggerService.CountAsync())}\nПри обнаружении аккаунта с положительным балансом его данные будут записаны в файл {_fileName}\nКоличество потоков будет определено автоматически в зависимости от системы и количества дериваций");
    }

    private static void ConsoleWriteImage(Image<Rgba32> image)
    {
        var sMax = 40;
        var percent = Math.Min(decimal.Divide(sMax, image.Width), decimal.Divide(sMax, image.Height));
        var resSize = new Size((int) (image.Width * percent), (int) (image.Height * percent));

        ConsoleColor ToConsoleColor(Rgba32 c)
        {
            int index = c.R > 128 | c.G > 128 | c.B > 128 ? 8 : 0;
            index |= c.R > 64 ? 4 : 0;
            index |= c.G > 64 ? 2 : 0;
            index |= c.B > 64 ? 1 : 0;
            return (ConsoleColor) index;
        }

        image.Mutate(x => x.Resize(resSize.Width * 2, resSize.Height * 2));

        Console.WriteLine("\n\n");
        for (int i = 0; i < resSize.Height; i++)
        {
            for (int j = 0; j < resSize.Width; j++)
            {
                Console.ForegroundColor = ToConsoleColor(image[j * 2, i * 2]);
                Console.BackgroundColor = ToConsoleColor(image[j * 2, i * 2 + 1]);
                Console.Write("▀");

                Console.ForegroundColor = ToConsoleColor(image[j * 2 + 1, i * 2]);
                Console.BackgroundColor = ToConsoleColor(image[j * 2 + 1, i * 2 + 1]);
                Console.Write("▀");
            }

            Console.WriteLine();
        }

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n\n");
    }

    private async Task Start(int countPerRequest, Cryptocurrency cryptocurrency)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Beep();
        double count = 0d, result = 0d;
        while (true)
        {
            var mnemonic = _mnemonicService.Generate(countPerRequest, cryptocurrency);

            Console.WriteLine($"({result:F} с) Обрабатывается мнемоническая фраза: {string.Join(" ", mnemonic.Words)}");
            var start = DateTime.Now;

            var tasks = mnemonic.Addresses.Select(address =>
                _balanceChecker.GetBalanceAsync(address.PublicAddress, cryptocurrency));

            BalanceInfo[] results;
            try
            {
                results = await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при отправке запросов: {ex.Message}");
                continue;
            }

            result = (DateTime.Now - start).TotalSeconds;
            for (int i = 0; i < countPerRequest; i++)
            {
                string text = _loggerService.CreateLogString(mnemonic.Addresses[i], results[i]);
                Console.WriteLine(text);
                Console.Title =
                    $"Всего найдено: {count.ToString("f16")} {cryptocurrency.ToString()}, потоков: {ThreadPool.ThreadCount}";
                if (results[i].Balance <= 0) continue;
                await _loggerService.LogToFileAsync(text);
                count += results[i].Balance;
            }
        }
    }
}