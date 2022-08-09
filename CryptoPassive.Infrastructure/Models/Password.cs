namespace CryptoPassive.Infrastructure.Models;

public class Password
{
    public long Id { get; set; }
    public string PasswordString { get; set; } = null!;
    public CryptoPassive.Infrastructure.Enums.Type Type { get; set; }
}