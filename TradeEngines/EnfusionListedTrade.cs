using System.Globalization;
using System.Reflection;

namespace BloombergTradeFeed.TradeEngines;

public class EnfusionListedTrade
{
    public EnfusionListedTrade()
    {
    }

    public EnfusionListedTrade(Type_TradeFeed trade)
    {
        var accountId = trade.Common.CustodySafekeepingNumber;
        TradeDate = trade.Common.TradeDate;
        SettleDate = trade.Common.SettlementDate;
        AccountId = trade.Common.CustodySafekeepingNumber ?? "432687";
        CounterpartyCode = trade.Common.AccountCounterpartyShortName;
        TransactionType = trade.Common.BuySellCoverShortFlag.ToEnfusionTransactionType();
        Identifier = GetTradeIdentifier(trade);
        IdentifierType = GetTradeIdentifierType(trade);
        Quantity = Convert.ToDouble(trade.Common.TradeAmount);
        TradeCCY = "USD";
        TradePrice = Convert.ToDouble(trade.Common.SettlementCcyPrice);
        SettleCCY = "USD";
        ExternalReference = trade.Common.OriginalTktId.ToString(CultureInfo.InvariantCulture);

        if (trade.TransactionCosts != null && trade.TransactionCosts.Length != 0)
            MapTransactionCosts(trade.TransactionCosts);
    }

    private static string GetTradeIdentifier(Type_TradeFeed trade)
    {
        if (trade.Common.SecurityProductKey == "9")
            return $"{trade.Common.SecurityIdentifier} {trade.Common.CouponStrikePrice:f0} Index";
        
        return !string.IsNullOrEmpty(trade.Common.OCCOptionTicker)
            ? trade.Common.OCCOptionTicker
            : trade.Common.SecurityIdentifier;
    }

    private static string GetTradeIdentifierType(Type_TradeFeed trade)
    {
        if (trade.Common.SecurityProductKey == "9")
            return "Bloomberg Yellow";
        
        return !string.IsNullOrEmpty(trade.Common.OCCOptionTicker)
            ? "OSI"
            : trade.Common.SecurityIdentifierFlag.ToEnfusionSecurityIdentifier();
    }

    public DateTime TradeDate { get; set; }
    public DateTime SettleDate { get; set; }
    public DateTime? OriginalTradeDate { get; set; }
    public DateTime? OriginalSettleDate { get; set; }
    public string AccountId { get; set; }
    public string CounterpartyCode { get; set; }
    public string TransactionType { get; set; }
    public string Identifier { get; set; }
    public string IdentifierType { get; set; }
    public double Quantity { get; set; }
    public string TradeCCY { get; set; }
    public double TradePrice { get; set; }
    public string SettleCCY { get; set; }
    public double? TradeBookFXRate { get; set; }
    public double? TradeSettleFXRate { get; set; }
    public double? OtherPayments1 { get; set; }
    public string OtherPayments1Type { get; set; }
    public bool? OtherPayments1Implied { get; set; }
    public double? OtherPayments2 { get; set; }
    public string OtherPayments2Type { get; set; }
    public bool? OtherPayments2Implied { get; set; }
    public double? OtherPayments3 { get; set; }
    public string OtherPayments3Type { get; set; }
    public bool? OtherPayments3Implied { get; set; }
    public double? OtherPayments4 { get; set; }
    public string OtherPayments4Type { get; set; }
    public bool? OtherPayments4Implied { get; set; }
    public double? OtherPayments5 { get; set; }
    public string OtherPayments5Type { get; set; }
    public bool? OtherPayments5Implied { get; set; }
    public double? OtherPayments6 { get; set; }
    public string OtherPayments6Type { get; set; }
    public bool? OtherPayments6Implied { get; set; }
    public double? OtherPayments7 { get; set; }
    public string OtherPayments7Type { get; set; }
    public bool? OtherPayments7Implied { get; set; }
    public double? OtherPayments8 { get; set; }
    public string OtherPayments8Type { get; set; }
    public bool? OtherPayments8Implied { get; set; }
    public double? OtherPayments9 { get; set; }
    public string OtherPayments9Type { get; set; }
    public bool? OtherPayments9Implied { get; set; }
    public double? OtherPayments10 { get; set; }
    public string OtherPayments10Type { get; set; }
    public bool? OtherPayments10Implied { get; set; }
    public string TraderName { get; set; }
    public string TradeType { get; set; }
    public int? TRSId { get; set; }
    public int? BookId { get; set; }
    public string BookPath { get; set; }
    public string BookName { get; set; }
    public string ExternalReference { get; set; }
    public string InternalReference { get; set; }
    public string Notes { get; set; }
    public string BookingStatus { get; set; }
    public string DealId { get; set; }
    public string TradeStatus { get; set; }
    public string CancelCorrectIdType { get; set; }
    public object CancelCorrectId { get; set; } // Integer or String
    public int? SettlementInstructionId { get; set; }
    public int? BorrowId { get; set; }
    public double? BorrowOverrideRate { get; set; }
    public DateTime? BorrowOverrideEffectiveDate { get; set; }
    public DateTime? TRSFinancingOverrideStartDate { get; set; }
    public DateTime? TRSFinancingOverrideTerminationDate { get; set; }
    public DateTime? TRSFinancingOverrideFinancingLegFirstResetDate { get; set; }
    public DateTime? TRSFinancingOverrideFinancingLegLastResetDate { get; set; }
    public string TRSFinancingOverrideFinancingLegResetFrequency { get; set; }
    public DateTime? TRSFinancingOverrideReturnLegFirstResetDate { get; set; }
    public DateTime? TRSFinancingOverrideReturnLegLastResetDate { get; set; }
    public string TRSFinancingOverrideFinancingLegBenchmarkIndexIdentifier { get; set; }
    public string TRSFinancingOverrideFinancingLegBenchmarkIndexIdentifierType { get; set; }
    public DateTime? TRSFinancingOverrideEffectiveDate { get; set; }
    public double? TRSFinancingOverrideRate { get; set; }
    public string TradeVenue { get; set; }
    public string TradeKeywordName1 { get; set; }
    public string TradeKeywordValue1 { get; set; }
    public DateTime? TradeKeywordDate1 { get; set; }
    public string TradeKeywordName2 { get; set; }
    public string TradeKeywordValue2 { get; set; }
    public DateTime? TradeKeywordDate2 { get; set; }
    public string TradeKeywordName3 { get; set; }
    public string TradeKeywordValue3 { get; set; }
    public DateTime? TradeKeywordDate3 { get; set; }
    public string TradeKeywordName4 { get; set; }
    public string TradeKeywordValue4 { get; set; }
    public DateTime? TradeKeywordDate4 { get; set; }
    public string TradeKeywordName5 { get; set; }
    public string TradeKeywordValue5 { get; set; }
    public DateTime? TradeKeywordDate5 { get; set; }
    public string TradeKeywordName6 { get; set; }
    public string TradeKeywordValue6 { get; set; }
    public DateTime? TradeKeywordDate6 { get; set; }
    public string TradeKeywordName7 { get; set; }
    public string TradeKeywordValue7 { get; set; }
    public DateTime? TradeKeywordDate7 { get; set; }
    public string TradeKeywordName8 { get; set; }
    public string TradeKeywordValue8 { get; set; }
    public DateTime? TradeKeywordDate8 { get; set; }
    public string TradeKeywordName9 { get; set; }
    public string TradeKeywordValue9 { get; set; }
    public DateTime? TradeKeywordDate9 { get; set; }
    public string TradeKeywordName10 { get; set; }
    public string TradeKeywordValue10 { get; set; }
    public DateTime? TradeKeywordDate10 { get; set; }
    public int? AllocationTemplateId { get; set; }
    public string AllocationAccountIds { get; set; }
    public string AllocationHierarchies { get; set; }
    public string AllocationBookPaths { get; set; }
    public string AllocationAmounts { get; set; }
    public string AllocationNumberType { get; set; }
    public string Description { get; set; }
    public bool? CancelCorrectBookNew { get; set; }
    public double? ReportedNativeProceeds { get; set; }
    public double? ReportedFXRate { get; set; }
    public double? ReportedGrossProceeds { get; set; }
    public double? ReportedBondTradeQuantity { get; set; }
    public double? ReportedBaseProceeds { get; set; }
    public double? ReportedSettlePrice { get; set; }
    public double? ReportedInitialResetRate { get; set; }
    public double? ReportedMarketValue { get; set; }
    public double? ReportedNotionalNetProceeds { get; set; }
    public bool? CancelCorrectBookIfNew { get; set; }
    public string Identifier2 { get; set; }
    public string Identifier3 { get; set; }
    public string Identifier4 { get; set; }
    public string Identifier5 { get; set; }
    public string IdentifierType2 { get; set; }
    public string IdentifierType3 { get; set; }
    public string IdentifierType4 { get; set; }
    public string IdentifierType5 { get; set; }
    public string CounterPartyKeywordName { get; set; }
    public string VendorSecurityId { get; set; }
    public string KettleName { get; set; }
    public string AllocationBorrowIds { get; set; }
    public string AllocationTRSIds { get; set; }
    public double? InitialMarginRate { get; set; }
    public string Ticker { get; set; }
    public string ExchangeCode { get; set; }
    public string ExchangeCodeType { get; set; }
    public string ContractKeywordName { get; set; }
    public string ContractKeywordValue { get; set; }
    public string ContractKeywordName2 { get; set; }
    public string ContractKeywordValue2 { get; set; }
    public string ContractKeywordName3 { get; set; }
    public string ContractKeywordValue3 { get; set; }
    public string ContractType { get; set; }
    public string ContractCCY { get; set; }
    public int? FutureMonth { get; set; }
    public int? FutureYear { get; set; }
    public string FinancialSubType { get; set; }
    public string PutCall { get; set; }
    public double? Strike { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? ExpirationDateTo { get; set; }
    public string ExcerciseType { get; set; }
    public double? ReportedLocalNotional { get; set; }
    public double? ReportedSettleNotional { get; set; }
    public double? ReportedSettleNetPrice { get; set; }
    public double? ReportedDividendPercent { get; set; }
    public string ReportedBenchmark { get; set; }
    public string ReportedMaturityDate { get; set; }
    public double? ReportedSpread { get; set; }
    public string ReportedInstrumentType { get; set; }
    public string PositionBlock { get; set; }

    public override string ToString()
    {
        var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var values = properties.Select(p =>
        {
            var value = p.GetValue(this);
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("d", CultureInfo.InvariantCulture);
            }

            return value?.ToString() ?? "";
        }).ToArray();
        return string.Join(",", values);
    }

    public static string GetCsvHeader()
    {
        return
            "Trade Date,Settle Date,Original Trade Date,Original Settle Date,Account Id,Counterparty Code,Transaction Type," +
            "Identifier,Identifier Type,Quantity,Trade CCY,Trade Price,Settle CCY,Trade/Book FX Rate,Trade/Settle FX Rate," +
            "Other Payments 1,Other Payments 1 Type,Other Payments 1 Implied,Other Payments 2,Other Payments 2 Type," +
            "Other Payments 2 Implied,Other Payments 3,Other Payments 3 Type,Other Payments 3 Implied,Other Payments 4," +
            "Other Payments 4 Type,Other Payments 4 Implied,Other Payments 5,Other Payments 5 Type,Other Payments 5 Implied," +
            "Other Payments 6,Other Payments 6 Type,Other Payments 6 Implied,Other Payments 7,Other Payments 7 Type," +
            "Other Payments 7 Implied,Other Payments 8,Other Payments 8 Type,Other Payments 8 Implied,Other Payments 9," +
            "Other Payments 9 Type,Other Payments 9 Implied,Other Payments 10,Other Payments 10 Type,Other Payments 10 Implied," +
            "Trader Name,Trade Type,TRS Id,Book Id,Book Path,Book Name,External Reference,Internal Reference,Notes,Booking Status," +
            "Deal Id,Trade Status,Cancel/Correct Id Type,Cancel/Correct Id,Settlement Instruction Id,Borrow Id,Borrow Override Rate ," +
            "Borrow Override Effective Date,TRS Financing Override Start Date,TRS Financing Override Termination Date," +
            "TRS Financing Override Financing Leg First Reset Date,TRS Financing Override Financing Leg Last Reset Date," +
            "TRS Financing Override Financing Leg Reset Frequency,TRS Financing Override Return Leg First Reset Date," +
            "TRS Financing Override Return Leg Last Reset Date,TRS Financing Override Financing Leg Benchmark Index Identifier," +
            "TRS Financing Override Financing Leg Benchmark Index Identifier Type,TRS Financing Override Effective Date," +
            "TRS Financing Override Rate,Trade Venue,Trade Keyword Name 1,Trade Keyword Value 1,Trade Keyword Date 1," +
            "Trade Keyword Name 2,Trade Keyword Value 2,Trade Keyword Date 2,Trade Keyword Name 3,Trade Keyword Value 3," +
            "Trade Keyword Date 3,Trade Keyword Name 4,Trade Keyword Value 4,Trade Keyword Date 4,Trade Keyword Name 5," +
            "Trade Keyword Value 5,Trade Keyword Date 5,Trade Keyword Name 6,Trade Keyword Value 6,Trade Keyword Date 6," +
            "Trade Keyword Name 7,Trade Keyword Value 7,Trade Keyword Date 7,Trade Keyword Name 8,Trade Keyword Value 8," +
            "Trade Keyword Date 8,Trade Keyword Name 9,Trade Keyword Value 9,Trade Keyword Date 9,Trade Keyword Name 10," +
            "Trade Keyword Value 10,Trade Keyword Date 10,Allocation Template Id,Allocation Account Ids,Allocation Hierarchies," +
            "Allocation Book Paths,Allocation Amounts,Allocation Number Type,Description,Cancel/Correct Book New,Reported Native Proceeds," +
            "Reported FX Rate,Reported Gross Proceeds,Reported Bond Trade Quantity,Reported Base Proceeds,Reported Settle Price," +
            "Reported Initial Reset Rate,Reported Market Value,Reported Notional Net Proceeds,Identifier 2,Identifier 3,Identifier 4," +
            "Identifier 5,Identifier Type 2,Identifier Type 3,Identifier Type 4,Identifier Type 5,Counter Party Keyword Name," +
            "Vendor Security Id,Allocation Borrow Ids,Allocation TRS Ids,Initial Margin Rate,Ticker,Exchange Code,Exchange Code Type," +
            "Contract Keyword Name,Contract Keyword Value,Contract Keyword Name 2,Contract Keyword Value 2,Contract Keyword Name 3," +
            "Contract Keyword Value 3,Contract Type,Contract CCY,Future Month,Future Year,Financial Subtype,Put Call,Strike,Expiration Date,Expiration Date To," +
            "Exercise Date,Reported Local Notional,Reported Settle Notional,Reported Settle Net Price,Reported Dividend Percent," +
            "Reported Benchmark,Reported Maturity Date,Reported Spread,Reported Instrument Type,Position Block,Asset/Liability Accr GL,Income/Expense Accr GL," +
            "Income/Expense Realized GL,Reported External Reference,Reported Local Gross Price,Reported Local Net Price,Reported Settle Gross Price," +
            "Reported Settle Gross Proceeds,Reported Order Date,Reported Cash Flow Type,Reported Clearing Counterparty,Reported Underlying Symbol," +
            "Reported Strike Price,Reported Option Type,Reported Exchange,Reported Trade Underlying Security ID,Reported Trade Contract Keyword Value," +
            "Execution Broker Name,Identifier 6,Identifier 7,Identifier 8,Identifier Type 6,Identifier Type 7,Identifier Type 8," +
            "TRS Financing Override Financing Leg Accrual Method,TRS Financing Override Financing Leg Payment Frequency,TRS Financing Override Financing Leg Lookback Days," +
            "TRS Financing Override Financing Leg Reset Rate Method,PBD Market Price,Trade Keyword Name 11,Trade Keyword Value 11,Trade Keyword Date 11," +
            "Trade Keyword Name 12,Trade Keyword Value 12,Trade Keyword Date 12,Trade Keyword Name 13,Trade Keyword Value 13,Trade Keyword Date 13,Trade Keyword Name 14," +
            "Trade Keyword Value 14,Trade Keyword Date 14,Trade Keyword Name 15,Trade Keyword Value 15,Trade Keyword Date 15,Trade Keyword Name 16,Trade Keyword Value 16," +
            "Trade Keyword Date 16,Trade Keyword Name 17,Trade Keyword Value 17,Trade Keyword Date 17,Trade Keyword Name 18,Trade Keyword Value 18,Trade Keyword Date 18," +
            "Trade Keyword Name 19,Trade Keyword Value 19,Trade Keyword Date 19,Trade Keyword Name 20,Trade Keyword Value 20,Trade Keyword Date 20";
    }

    private void MapTransactionCosts(IEnumerable<Type_TransactionCost> transactionCosts)
    {
        var transactions = transactionCosts?.ToList() ?? [];
        for (var i = 0; i < transactions.Count; i++)
        {
            var cost = transactions.ElementAt(i);

            var otherPaymentsProperties = GetType().GetProperties()
                .FirstOrDefault(p => p.Name.StartsWith($"OtherPayments{i + 1}") && p.PropertyType == typeof(double?));

            var otherPaymentsTypeProperties = GetType().GetProperties()
                .FirstOrDefault(p => p.Name.StartsWith($"OtherPayments{i + 1}") && p.PropertyType == typeof(string));

            var otherPaymentsImpliedProperties = GetType().GetProperties()
                .FirstOrDefault(p => p.Name.StartsWith($"OtherPayments{i + 1}") && p.PropertyType == typeof(bool?));

            switch (cost.Type)
            {
                case "2":
                    otherPaymentsProperties?.SetValue(this, Convert.ToDouble(cost.Rate));
                    otherPaymentsTypeProperties?.SetValue(this, "PerShareCommission");
                    otherPaymentsImpliedProperties?.SetValue(this, false);
                    break;
                case "3":
                    otherPaymentsTypeProperties?.SetValue(this, "ExchangeFeeAutoCalc");
                    otherPaymentsImpliedProperties?.SetValue(this, false);
                    break;
                case "8":
                    otherPaymentsProperties?.SetValue(this, Convert.ToDouble(cost.Rate));
                    otherPaymentsTypeProperties?.SetValue(this, "ClearingFee");
                    otherPaymentsImpliedProperties?.SetValue(this, false);
                    break;
                case "16":
                    otherPaymentsProperties?.SetValue(this, Convert.ToDouble(cost.Rate));
                    otherPaymentsTypeProperties?.SetValue(this, "OtherFee");
                    otherPaymentsImpliedProperties?.SetValue(this, false);
                    break;
                default:
                    throw new Exception("Unsupported transaction");
                    break;
            }
        }
    }
}