namespace ConsolidationService.Domain.Entities;

public class DailyBalance
{
    public string MerchantId { get; private set; }
    public DateOnly Date { get; private set; }
    public decimal TotalCredit { get; private set; }
    public decimal TotalDebit { get; private set; }
    public decimal Balance { get; private set; }

    private DailyBalance() { MerchantId = string.Empty; }

    public DailyBalance(string merchantId, DateOnly date, decimal totalCredit, decimal totalDebit)
    {
        MerchantId = merchantId;
        Date = date;
        TotalCredit = totalCredit;
        TotalDebit = totalDebit;
        Balance = totalCredit - totalDebit;
    }
}
