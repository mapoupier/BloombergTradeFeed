using BloombergTradeFeed.TradeEngines;
using TestXmlParser.TradeEngines;

namespace TestXmlParser;

public class Program
{
    public static void Main(string[] args)
    {
        var folderPath = @"C:\temp\trades";
        var archivePath = Path.Combine(folderPath, "archive");

        if (!Directory.Exists(archivePath))
        {
            Directory.CreateDirectory(archivePath);
        }

        var files = Directory.EnumerateFiles(folderPath, "*.xml").ToList();

        var tradeEngines = new List<TradeEngine>
        {
            new GoldmanSachsTrades("/var/forza/ftpaccounts/goldman/outgoing/"),
            //new EnfusionTrades()
            // Add new trade engines here
        };

        foreach (var engine in tradeEngines)
        {
            engine.ProcessTrades(files);
        }

        //ArchiveFiles(files, archivePath);
    }

    private static void ArchiveFiles(IEnumerable<string> files, string archivePath)
    {
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var archiveFile = Path.Combine(archivePath, fileName);
            File.Move(file, archiveFile);
        }
    }
}