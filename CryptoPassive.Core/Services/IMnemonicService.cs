using CryptoPassive.Core.Enums;
using CryptoPassive.Core.Models;

namespace CryptoPassive.Core.Services;

public interface IMnemonicService
{
    public Mnemonic Generate(int count, Cryptocurrency cryptocurrency);
}