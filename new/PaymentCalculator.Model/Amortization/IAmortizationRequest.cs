namespace PaymentCalculator.Model.Amortization
{
    public interface IAmortizationRequest
    {
        decimal TotalPrincipal { get; }
        int NumberOfPeriods { get; }
        decimal RatePerPeriod { get; }
    }
}
