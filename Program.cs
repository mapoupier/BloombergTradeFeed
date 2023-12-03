// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Xml.Serialization;
using TestXmlParser;

Console.WriteLine("Starting");
var serializer = new XmlSerializer(typeof(Type_TradeFeed));
var folderPath = @"C:\temp\trades"; // Replace with your folder path
foreach (var file in Directory.EnumerateFiles(folderPath, "*.xml"))
{
    using var fileStream = new FileStream(file, FileMode.Open);
    var result = (Type_TradeFeed)serializer.Deserialize(fileStream);

    if (result is null)
    {
        Console.WriteLine("Unable to read file");
        return -1;
    }

    var reportDate = new DateTime(2023, 11, 29).DayOfYear;
    var fileDate = result.Common.TradeDate.Date.DayOfYear;
    if (fileDate != reportDate) continue; 
    Console.WriteLine(string.Join(", ",
        new List<string>
        {
            // file,
            result.Common.TransactionNumber.ToString(CultureInfo.InvariantCulture), // Order Number
            "N", // Cancel Correct Indicator
            result.Common.AccountCounterpartyShortName, // Account Number
            !string.IsNullOrEmpty(result.Common.OCCOptionTicker) ? result.Common.OCCOptionTicker : result.Common.SecurityIdentifier, // Security Identifier
            result.Common.CustomerAccountCounterparty, // Broker
            "GSCO", // Custodian
            result.Common.BuySellCoverShortFlag.ToGoldmanTransactionType(), // Transaction Type
            result.Common.SecurityCurrencyISOCode, // Current code
            result.Common.TradeDate.ToString("d", CultureInfo.InvariantCulture), // Trade Date
            result.Common.SettlementDate.ToString("d", CultureInfo.InvariantCulture), // Settle Date
            result.Common.TradeAmount.ToString("F2",CultureInfo.InvariantCulture), // Quantity
            result.Common.SettlementCcyTotalCommission.ToString("F6",CultureInfo.InvariantCulture), // Commission
            result.Common.SettlementCcyPrice.ToString("F8",CultureInfo.InvariantCulture), // Price
            "", // Accrued interest 
            "", // Trade tax 
            "", // Misc money 
            "", // Net amount 
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
            "" // Trailer 5
        }));
}

Console.WriteLine("Finished");

return 0;