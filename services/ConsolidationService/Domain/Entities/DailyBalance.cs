namespace ConsolidationService.Domain.Entities;

public class DailyBalance
{
    public Guid Id { get; private set; }
    public Guid MerchantId { get; private set; }
    public DateTime Date { get; private set; }
    public decimal TotalCredit { get; private set; }
    public decimal TotalDebit { get; private set; }
    public decimal Balance { get; private set; }

    private DailyBalance() { }

    public DailyBalance(Guid merchantId, DateTime date)
    {
        Id = Guid.NewGuid();
        MerchantId = merchantId;
        Date = date.Date;
        TotalCredit = 0;
        TotalDebit = 0;
        Balance = 0;
    }

    public void AddCredit(decimal amount)
    {
        TotalCredit += amount;
        UpdateBalance();
    }

    public void AddDebit(decimal amount)
    {
        TotalDebit += amount;
        UpdateBalance();
    }

    private void UpdateBalance()
    {
        Balance = TotalCredit - TotalDebit;
    }
}
