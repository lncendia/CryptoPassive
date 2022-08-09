namespace CryptoPassive.Core.Models;

public class Mnemonic
{
    public Mnemonic(IList<string> words, IList<Address> addresses)
    {
        Words = words;
        Addresses = addresses;
    }

    public IList<string> Words { get; }
    public IList<Address> Addresses { get; }
}