using System.Xml.Serialization;

namespace TestXmlParser;

public abstract class TradeEngine
{
    private readonly XmlSerializer _serializer = new XmlSerializer(typeof(Type_TradeFeed));
    public abstract void ProcessTrades(IEnumerable<string> files);

    protected string GetSecurityProductKeyString(string commonSecurityProductKey)
    {
        return commonSecurityProductKey switch
        {
            "1" => "Commodity",
            "2" => "Equity",
            "3" => "Municipals",
            "4" => "Preferred",
            "6" => "Money Market",
            "7" => "Government",
            "8" => "Corporate",
            "9" => "Index",
            "10" => "Currency",
            "11" => "Mortgage",
            _ => "NotFound"
        };
    }

    protected Type_TradeFeed DeserializeTradeFile(string file)
    {
        using var fileStream = new FileStream(file, FileMode.Open);
        var result = (Type_TradeFeed)_serializer.Deserialize(fileStream)!;
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to read file: {file}");
        }

        return result;
    }
}