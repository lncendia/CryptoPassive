namespace CryptoPassive.Core.Models;

public class BalanceInfo
{
    public BalanceInfo(double balance, int transactionCount)
    {
        TransactionCount = transactionCount;
        Balance = balance;
    }

    public double Balance { get; }
    public int TransactionCount { get; }
}