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

    public static string ToProductKeyToBloombergShortKey(this string str)
    {
        return str switch
        {
            "1" => "CMDT",
            "2" => "EQTY",
            "3" => "MUNI",
            "4" => "PRFD",
            "5" => "CLNT",
            "6" => "M-MKT",
            "7" => "GOVT",
            "8" => "CORP",
            "9" => "INDX",
            "10" => "CURR",
            "11" => "MTGE",
            _ => str
        };
    }

    public static string ToProductKeyToBloombergProductSubFlag(this string str, string subFlag)
    {
        return str switch
        {
            "1" => subFlag switch
            {
                "2" => "Future",
                "6" => "Option",
                "10" => "Spot object (example: gold)",
                "102" => "HardCommodities/Currency Futures",
                "106" => "HardCommodities/Currency Future Options",
                _ => "Unknown SubFlag"
            },
            "2" => subFlag switch
            {
                "0" => "Common",
                "2" => "Receipts",
                "3" => "Warrants",
                "4" => "Unit",
                "5" => "Amherst",
                "6" => "Option",
                "7" => "EQPL (Private/Synthetic Equity Security)",
                "8" => "Mutual Fund",
                "9" => "Right",
                "10" => "Money market",
                "11" => "Closed end fund",
                "12" => "UK unit trust",
                "13" => "Offshore fund",
                "14" => "Preferreds",
                "15" => "Convertible bonds",
                "16" => "Other (only securities found under <EQTY> TK WEB)",
                "18" => "Investment Fund",
                "19" => "UIT",
                "20" => "REIT",
                "24" => "Convertible Preferreds",
                "26" => "Private companies",
                "27" => "Bond",
                "29" => "OTC Option/Synthetic",
                "30" => "Preferred shares",
                "31" => "Defunct",
                "37" => "custombskt",
                "38" => "Limited partners",
                "39" => "Carry Forward",
                "40" => "ETF",
                "41" => "Tracking Stock",
                "42" => "Equity Future",
                "43" => "Royalty Trust",
                "44" => "fdic",
                "45" => "struct_term",
                "46" => "security_lending",
                "50" => "ISUP",
                "68" => "otc_opvol",
                "69" => "eqtyfut_spread",
                "70" => "SUBFLAG_PAC",
                "71" => "STAPLED",
                _ => "Unknown SubFlag"
            },
            "3" => subFlag switch
            {
                "0" => "Muni",
                _ => "Unknown SubFlag"
            },
            "4" => subFlag switch
            {
                "1" => "Preferreds",
                _ => "Unknown SubFlag"
            },
            "5" => subFlag switch
            {
                _ => "Unknown SubFlag"
            },
            "6" => subFlag switch
            {
                "12" => "MMKT",
                "14" => "Mexican MMKT",
                "18" => "Repo",
                "19" => "MMKT Commitment",
                "22" => "Reverse Repo",
                "23" => "Deposit",
                "100" => "MMKT Program",
                _ => "Unknown SubFlag"
            },
            "7" => subFlag switch
            {
                "0" => "Basic",
                "1" => "WI",
                "2" => "Generic",
                "3" => "UKT",
                "29" => "OTC option",
                _ => "Unknown SubFlag"
            },
            "8" => subFlag switch
            {
                "1" => "Basic",
                "2" => "Tiger",
                "3" => "UKT",
                "4" => "Dead Bonds",
                "5" => "Fixed Warrants",
                "6" => "Percent Warrants",
                "7" => "PRPL",
                "8" => "OTC Futures",
                "9" => "Swap",
                "10" => "Basic/For Internal Bloomberg use",
                "11" => "Basic/For Internal Bloomberg use",
                "13" => "For Internal Bloomberg use",
                "15" => "For Internal Bloomberg use",
                "16" => "Non-US Govt Generics",
                "17" => "Synthetics",
                "29" => "OTC Option",
                "98" => "Structured Note",
                "99" => "Loan",
                _ => "Unknown SubFlag"
            },
            "9" => subFlag switch
            {
                "2" => "Equity",
                "6" => "Spot Index Options/Index Future Options",
                _ => "Unknown SubFlag"
            },
            "10" => subFlag switch
            {
                "29" => "OTC Option",
                "30" => "Forward and spot",
                "2" => "Future",
                "6" => "Option",
                "102" => "HardCommodities/Currency Futures",
                "106" => "HardCommodities/Currency Future Options",
                _ => "Unknown SubFlag"
            },
            "11" => subFlag switch
            {
                "1" => "Generics",
                "2" => "Pool_GN",
                "3" => "Pool_G2",
                "4" => "Pool_FH",
                "5" => "Pool_FN",
                "6" => "Pool_FG",
                "16" => "CMO & ABS’s",
                "19" => "Private (Pool PRPLs)",
                "20" => "Private CMO (PRPLs)",
                "21" => "TBA",
                "22" => "AMSEC (Canadian mortgages and SBA’s)",
                "23" => "TBA – Coupon Swap",
                "24" => "TBA – Month Roll",
                "30" => "Japanese Securities",
                "31" => "STRU",
                _ => "Unknown SubFlag"
            },
            _ => str
        };
    }

    public static string ToGoldmanTradeIndicator(this string str)
    {
        return str switch
        {
            "Y" or "102" => "C",
            "202" => "A",
            _ => "N"
        };
    }
}