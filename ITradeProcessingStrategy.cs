namespace BloombergTradeFeed;

public interface ITradeProcessingStrategy
{
    string ToTransactionType(string str);
    string ToTradeIndicator(string str);
}