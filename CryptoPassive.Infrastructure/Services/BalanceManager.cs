using CryptoPassive.Core.Models;
using CryptoPassive.Core.Services;
using NBitcoin;
using Nethereum.Web3;
using QBitNinja.Client;

namespace CryptoPassive.Infrastructure.Services;

public class BalanceManager : IBalanceManager
{
    public async Task<BalanceInfo> GetBitcoinBalanceAsync(string address)
    {
        var client = new QBitNinjaClient(Network.Main);
        var balance = await client.GetBalanceSummary(BitcoinAddress.Create(address, Network.Main));
        return new BalanceInfo(balance.Confirmed.Amount.Satoshi / 100000000d, balance.Confirmed.TransactionCount);
    }

    public async Task<BalanceInfo> GetEthereumBalanceAsync(string address)
    {
        var web3 = new Web3("https://cloudflare-eth.com/");
        var txCount = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address,
            web3.Eth.Transactions.GetTransactionCount.DefaultBlock);
        var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
        var etherAmount = Web3.Convert.FromWei(balance.Value);
        await Task.Delay(900);
        return new BalanceInfo((double) etherAmount, (int) txCount.Value);
    }
}