using System.Globalization;
using Rebex.Net;

namespace BloombergTradeFeed.TradeEngines;

public class GoldmanSachsTrades(string localOutputPath) : TradeEngine
{
    private static readonly GoldmanSachsTradeProcessingStrategy TradeProcessingStrategy = new();
    private static readonly Dictionary<string, string> ProcessedTrades = new();
    private readonly string _outputFileName = Path.Combine(localOutputPath, $"tradefile.brean.{DateTime.Now:yyyyMMddHHmm}.csv");

    private List<Type_TradeFeed> ScanTrades(IEnumerable<string> files)
    {
        var trades = new List<Type_TradeFeed>();
        foreach (var file in files)
        {
            var trade = DeserializeTradeFile(file);
            if (ShouldProcessTrade(trade))
            {
                trades.Add(trade);
            }
        }

        return trades;
    }

    private static void IdentifyExclusions(List<Type_TradeFeed> trades)
    {
        foreach (var trade in trades)
        {
            var tradeIndicator = TradeProcessingStrategy.ToTradeIndicator(trade.Common.RecordType);
            var orderNumber = trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture);

            if (ProcessedTrades.TryGetValue(orderNumber, out var value))
            {
                if (value == "N" && tradeIndicator == "C")
                {
                    ProcessedTrades.Remove(orderNumber); // Mark the original trade for exclusion
                }
            }
            else
            {
                ProcessedTrades[orderNumber] = tradeIndicator; // Add the new trade
            }
        }
    }
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

    private void ProcessTrade(Type_TradeFeed trade)
    {
            var tradeIndicator = TradeProcessingStrategy.ToTradeIndicator(trade.Common.RecordType);
            var orderNumber = trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture);

            if (ProcessedTrades.ContainsKey(orderNumber) && ProcessedTrades[orderNumber] == tradeIndicator)
            {
                var tradeDetails = string.Join(", ",
                    new List<string>
                    {
                        orderNumber, // Order Number
                        tradeIndicator, // Cancel Correct Indicator
                        "065450793", // Account Number
                        !string.IsNullOrEmpty(trade.Common.OCCOptionTicker)
                            ? trade.Common.OCCOptionTicker
                            : trade.Common.SecurityIdentifier, // Security Identifier
                        trade.Common.CustomerAccountCounterparty, // Broker
                        "GSCO", // Custodian
                        TradeProcessingStrategy.ToTransactionType(trade.Common.BuySellCoverShortFlag), // Transaction Type
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
                    });

                File.AppendAllText(_outputFileName, tradeDetails + Environment.NewLine);
            }
    }
    private void UploadFileToFtp(string filePath)
    {
        var ftpHost = Environment.GetEnvironmentVariable($"TRADEFEED_GSCO_HOST");
        var ftpUser = Environment.GetEnvironmentVariable($"TRADEFEED_GSCO_USER");
        var ftpPass = Environment.GetEnvironmentVariable($"TRADEFEED_GSCO_PASS");

        if (ftpHost is null || ftpUser is null || ftpPass is null)
        {
            Console.WriteLine("FTP CREDENTIALS ARE NOT AVAILABLE");
            return;
        }
        var sftp = new Sftp();
        try
        {
            sftp.Connect(ftpHost);
            sftp.Login(ftpUser, ftpPass);
            sftp.PutFile(filePath, Path.GetFileName(filePath));
        }
        finally
        {
            sftp.Disconnect();
        }
    }

    public override void ProcessTrades(IEnumerable<string> files)
    {
        if (!Path.Exists(localOutputPath))
            Directory.CreateDirectory(localOutputPath);
        
        var trades = ScanTrades(files);
        
        IdentifyExclusions(trades);

        foreach (var trade in trades)
        {
            var orderNumber = trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture);
            if (ProcessedTrades.ContainsKey(orderNumber))
                ProcessTrade(trade);
        }

        // Upload the file to the FTP server after processing all trades
        UploadFileToFtp(_outputFileName);
    }
}