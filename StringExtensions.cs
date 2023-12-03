namespace TestXmlParser;

public static class StringExtensions
{
    public static string ToGoldmanTransactionType(this string str)
    {
        return str switch
        {
            "C" => "BC",
            "H" => "SS",
            _ => str
        };
    }
}