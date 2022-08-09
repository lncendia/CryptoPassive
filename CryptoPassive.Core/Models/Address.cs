using CryptoPassive.Core.Enums;

namespace CryptoPassive.Core.Models;

public class Address
{
    public Address(string privateKey, string publicKey, string publicAddress, Cryptocurrency type)
    {
        PrivateKey = privateKey;
        PublicKey = publicKey;
        PublicAddress = publicAddress;
        Type = type;
    }

    public Cryptocurrency Type { get; }
    public string PrivateKey { get; }
    public string PublicKey { get; }
    public string PublicAddress { get; }
}