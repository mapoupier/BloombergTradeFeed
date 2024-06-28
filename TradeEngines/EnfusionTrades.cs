namespace TestXmlParser.TradeEngines;

public class EnfusionTrades : TradeEngine
{
    private static bool ShouldProcessTrade(Type_TradeFeed trade)
    {
        // Define the criteria for EnfusionTrades
        // For example purposes, we'll return true for all trades.
        return true;
    }

    private static void ProcessTrade(Type_TradeFeed trade)
    {
        // Define the processing logic for EnfusionTrades
        Console.WriteLine("Processing Enfusion Trade");
    }

    public override void ProcessTrades(IEnumerable<string> files)
    {
        foreach (var file in files)
        {
            var trade = DeserializeTradeFile(file);
            if (ShouldProcessTrade(trade))
            {
                ProcessTrade(trade);
            }
        }
    }
}