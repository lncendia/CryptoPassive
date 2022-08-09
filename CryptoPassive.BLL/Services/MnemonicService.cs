using CryptoPassive.Core.Enums;
using CryptoPassive.Core.Models;
using CryptoPassive.Core.Services;
using NBitcoin;
using Mnemonic = CryptoPassive.Core.Models.Mnemonic;

namespace CryptoPassive.BLL.Services;

public class MnemonicService : IMnemonicService
{
    private const string BitcoinPath = "m/0\'/0\'";
    private const string EthereumPath = "m/44\'/60\'/0\'/0/x";

    private static Address GetDerivedAddress(string privateExtendedKey, string path)
    {
        var key = new BitcoinExtKey(privateExtendedKey, Network.Main).Derive(new KeyPath(path));
        return new Address(key.PrivateKey.GetWif(Network.Main).ToWif(), key.PrivateKey.PubKey.ToString(),
            key.PrivateKey.GetAddress(ScriptPubKeyType.Segwit, Network.Main).ToString(), Cryptocurrency.Bitcoin);
    }

    private static string GetExtAddress(string privateExtendedKey, string path) =>
        new BitcoinExtKey(privateExtendedKey, Network.Main).Derive(new KeyPath(path)).ToWif();


    private static Mnemonic GenerateBitcoin(int count)
    {
        var mnemonic = new NBitcoin.Mnemonic(Wordlist.English, WordCount.Twelve);
        var baseExt = GetExtAddress(mnemonic.DeriveExtKey().GetWif(Network.Main).ToString(), BitcoinPath);
        var addresses = new List<Address>();
        for (var i = 0; i < count; i++) addresses.Add(GetDerivedAddress(baseExt, i.ToString()));
        return new Mnemonic(mnemonic.Words, addresses);
    }

    private static Mnemonic GenerateEthereum(int count)
    {
        // ReSharper disable once RedundantArgumentDefaultValue
        Nethereum.HdWallet.Wallet mnemonic = new(Wordlist.English, WordCount.Twelve, null, EthereumPath);
        var addresses = new List<Address>();
        for (var i = 0; i < count; i++)
        {
            var address = mnemonic.GetAccount(i);
            addresses.Add(new Address(address.PrivateKey, address.PublicKey, address.Address, Cryptocurrency.Ethereum));
        }

        return new Mnemonic(mnemonic.Words, addresses);
    }

    public Mnemonic Generate(int count, Cryptocurrency cryptocurrency)
    {
        return cryptocurrency switch
        {
            Cryptocurrency.Bitcoin => GenerateBitcoin(count),
            Cryptocurrency.Ethereum => GenerateEthereum(count),
            _ => throw new ArgumentOutOfRangeException(nameof(cryptocurrency), cryptocurrency, null)
        };
    }
}