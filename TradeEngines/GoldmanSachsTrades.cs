using System.Globalization;

namespace TestXmlParser.TradeEngines;

public class GoldmanSachsTrades : TradeEngine
{
    private static bool ShouldProcessTrade(Type_TradeFeed trade)
    {
        var excludedProduct = new[] { "CORP/Swap" };
        string[] recordTypes = ["2", "6", "102", "202"];
        var productType =
            $"{trade.Common.SecurityProductKey.ToProductKeyToBloombergShortKey()}/{trade.Common.SecurityProductKey.ToProductKeyToBloombergProductSubFlag(trade.Common.ProductSubFlag)}";

        return recordTypes.Contains(trade.Common.RecordType) &&
               trade.Common.CustodySafekeepingNumber == "065450793" &&
               !string.IsNullOrEmpty(trade.Common.CustomerAccountCounterparty) &&
               !excludedProduct.Contains(productType);
    }

    private static void ProcessTrade(Type_TradeFeed trade)
    {
        Console.WriteLine(string.Join(", ",
            new List<string>
            {
                trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture), // Order Number
                trade.Common.RecordType.ToGoldmanTradeIndicator(), // Cancel Correct Indicator
                "065450793", // Account Number
                !string.IsNullOrEmpty(trade.Common.OCCOptionTicker)
                    ? trade.Common.OCCOptionTicker
                    : trade.Common.SecurityIdentifier, // Security Identifier
                trade.Common.CustomerAccountCounterparty, // Broker
                "GSCO", // Custodian
                trade.Common.BuySellCoverShortFlag.ToGoldmanTransactionType(), // Transaction Type
                trade.Common.SecurityCurrencyISOCode, // Current code
                trade.Common.TradeDate.ToString("d", CultureInfo.InvariantCulture), // Trade Date
                trade.Common.SettlementDate.ToString("d", CultureInfo.InvariantCulture), // Settle Date
                trade.Common.TradeAmount.ToString("F2", CultureInfo.InvariantCulture), // Quantity
                trade.Common.SettlementCcyTotalCommission.ToString("F6", CultureInfo.InvariantCulture), // Commission
                trade.Common.SettlementCcyPrice.ToString("F8", CultureInfo.InvariantCulture), // Price
                "", // Accrued interest 
                "", // Trade tax 
                "", // Misc money 
                trade.Common.TotalTradeAmount.ToString("F2", CultureInfo.InvariantCulture), // Net amount 
                "", // Principal 
                "", // Description
                "", // Security Type
                "", // Country Settlement Code
                "", // Clearing Agent
                "", // SEC Fees
                "", // Option underlyer
                "", // Option expiry date
                "", // Option call put indicator
                "", // Option strike price
                "", // Trailer
                "", // Trailer 1
                "", // Trailer 2
                "", // Trailer 3
                "", // Trailer 4
                $"{trade.Common.SecurityProductKey.ToProductKeyToBloombergShortKey()}/{trade.Common.SecurityProductKey.ToProductKeyToBloombergProductSubFlag(trade.Common.ProductSubFlag)}" // Trailer 5
            }));
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