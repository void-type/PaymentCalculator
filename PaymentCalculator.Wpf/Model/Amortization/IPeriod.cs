namespace PaymentCalculator.Wpf.Model.Amortization
{
    public interface IPeriod
    {
        decimal BalanceLeft { get; set; }
        decimal InterestPayment { get; set; }
        int PeriodNumber { get; set; }
        decimal PrincipalPayment { get; set; }
    }
}
