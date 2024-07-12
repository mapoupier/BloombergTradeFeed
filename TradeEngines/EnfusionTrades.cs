using Rebex.Net;

namespace BloombergTradeFeed.TradeEngines;

public class EnfusionTrades(string localOutputPath) : TradeEngine
{
    private readonly string _outputFileName =
        Path.Combine(localOutputPath, $"trades.forza.{DateTime.Now:yyyyMMddHHmm}.csv");
    
    private static bool ShouldProcessTrade(Type_TradeFeed trade)
    {
        var excludedProduct = new[] { "CORP/Swap" };
        string[] recordTypes = ["2", "6", "102", "202"];
        var productType =
            $"{trade.Common.SecurityProductKey.ToProductKeyToBloombergShortKey()}/{trade.Common.SecurityProductKey.ToProductKeyToBloombergProductSubFlag(trade.Common.ProductSubFlag)}";

        return recordTypes.Contains(trade.Common.RecordType) &&
               !string.IsNullOrEmpty(trade.Common.CustomerAccountCounterparty) &&
               !excludedProduct.Contains(productType);
    }

    private void ProcessTrade(Type_TradeFeed trade)
    {
        
        var enfusionTrade = new EnfusionListedTrade(trade);
        File.AppendAllText(_outputFileName, enfusionTrade + Environment.NewLine);
    }
    
    public override void ProcessTrades(IEnumerable<string> files)
    {
        if (!Path.Exists(localOutputPath))
            Directory.CreateDirectory(localOutputPath);

        var trades = ScanAndFilterTrades(files);
        File.AppendAllText(_outputFileName, EnfusionListedTrade.GetCsvHeader() + Environment.NewLine);
        foreach (var trade in trades)
            ProcessTrade(trade);

        // Upload the file to the FTP server after processing all trades
        UploadFileToFtp(_outputFileName);
    }

    private static void UploadFileToFtp(string filePath)
    {
        var ftpHost = Environment.GetEnvironmentVariable($"TRADEFEED_ENFUSION_HOST");
        var ftpUser = Environment.GetEnvironmentVariable($"TRADEFEED_ENFUSION_USER");
        var ftpPass = Environment.GetEnvironmentVariable($"TRADEFEED_ENFUSION_PASS");
        var ftpDestFolder = Environment.GetEnvironmentVariable($"TRADEFEED_ENFUSION_DESTFOLDER");
        
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

    private IEnumerable<Type_TradeFeed> ScanAndFilterTrades(IEnumerable<string> files)
    {
        // Create an empty list to hold the filtered trades.
        var filteredTrades = new List<Type_TradeFeed>();

        // Loop through each file in the input files.
        foreach (var file in files)
        {
            var trade = DeserializeTradeFile(file);
            if (ShouldProcessTrade(trade))
                filteredTrades.Add(trade);
        }

        // Return the filtered trades.
        return filteredTrades;
    }
}