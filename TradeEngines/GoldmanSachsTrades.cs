using System.Globalization;
using Rebex.Net;

namespace BloombergTradeFeed.TradeEngines;

public class GoldmanSachsTrades(string localOutputPath) : TradeEngine
{
    private static readonly GoldmanSachsTradeProcessingStrategy TradeProcessingStrategy = new();

    private readonly string _outputFileName =
        Path.Combine(localOutputPath, $"tradefile.brean.{DateTime.Now:yyyyMMddHHmm}.csv");

    private bool ShouldProcessTrade(Type_TradeFeed trade)
    {
        var excludedProduct = new[] { "CORP/Swap" };
        string[] recordTypes = ["2", "6", "102", "202"];
        var productType =
            $"{trade.Common.SecurityProductKey.ToProductKeyToBloombergShortKey()}/{trade.Common.SecurityProductKey.ToProductKeyToBloombergProductSubFlag(trade.Common.ProductSubFlag)}";

        return recordTypes.Contains(trade.Common.RecordType) &&
               trade.Common.CustodySafekeepingNumber == "434544" &&
               !string.IsNullOrEmpty(trade.Common.CustomerAccountCounterparty) &&
               !excludedProduct.Contains(productType);
    }

    private List<Type_TradeFeed> ScanAndFilterTrades(IEnumerable<string> files)
    {
        var validTrades = new List<Type_TradeFeed>();
        var tradeStatuses = new Dictionary<string, List<string>>();

        foreach (var file in files)
        {
            var trade = DeserializeTradeFile(file);
            if (!ShouldProcessTrade(trade))
            {
                continue;
            }

            var tradeIndicator = TradeProcessingStrategy.ToTradeIndicator(trade.Common.RecordType);
            var orderNumber = trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture);

            if (!tradeStatuses.ContainsKey(orderNumber))
            {
                tradeStatuses[orderNumber] = [];
            }

            tradeStatuses[orderNumber].Add(tradeIndicator);
            validTrades.Add(trade);
        }

        // Identify and exclude canceled trades
        var tradesToExclude = new HashSet<Type_TradeFeed>();
        foreach (var orderNumber in tradeStatuses.Keys)
        {
            var indicators = tradeStatuses[orderNumber];
            var cancelIndices = new List<int>();

            for (int i = 0; i < indicators.Count; i++)
            {
                if (indicators[i] == "C" && cancelIndices.Count > 0)
                {
                    cancelIndices.Add(i);
                    tradesToExclude.Add(validTrades.First(trade =>
                        trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture) == orderNumber &&
                        TradeProcessingStrategy.ToTradeIndicator(trade.Common.RecordType) == "N"));
                    tradesToExclude.Add(validTrades.First(trade =>
                        trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture) == orderNumber &&
                        TradeProcessingStrategy.ToTradeIndicator(trade.Common.RecordType) == "C"));
                    cancelIndices.RemoveAt(cancelIndices.Count - 1);
                }
                else if (indicators[i] == "N")
                {
                    cancelIndices.Add(i);
                }
            }
        }

        validTrades.RemoveAll(trade => tradesToExclude.Contains(trade));
        return validTrades;
    }

    private void ProcessTrade(Type_TradeFeed trade)
    {
        var tradeIndicator = TradeProcessingStrategy.ToTradeIndicator(trade.Common.RecordType);
        var orderNumber = trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture);

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
                trade.Common.SettlementCcyTotalCommission.ToString("F6",
                    CultureInfo.InvariantCulture), // Commission
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

    private void UploadFileToFtp(string filePath)
    {
        var ftpHost = Environment.GetEnvironmentVariable($"TRADEFEED_GSCO_HOST");
        var ftpUser = Environment.GetEnvironmentVariable($"TRADEFEED_GSCO_USER");
        var ftpPass = Environment.GetEnvironmentVariable($"TRADEFEED_GSCO_PASS");
        var ftpDestFolder = Environment.GetEnvironmentVariable($"TRADEFEED_GSCO_DESTFOLDER");
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
            sftp.PutFile(filePath, Path.Join(ftpDestFolder, Path.GetFileName(filePath)));
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

        var trades = ScanAndFilterTrades(files);

        foreach (var trade in trades)
            ProcessTrade(trade);

        // Upload the file to the FTP server after processing all trades
        UploadFileToFtp(_outputFileName);
    }
}