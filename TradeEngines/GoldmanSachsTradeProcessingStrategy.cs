namespace BloombergTradeFeed.TradeEngines;

public class GoldmanSachsTradeProcessingStrategy : ITradeProcessingStrategy
{
    public string ToTransactionType(string str)
    {
        return str switch
        {
            "C" => "BC",
            "H" => "SS",
            _ => str
        };
    }

    public string ToTradeIndicator(string str)
    {
        return str switch
        {
            "Y" or "102" => "C",
            "202" => "A",
            _ => "N"
        };
    }
}