namespace PaymentCalculator.Model.Amortization
{
    public class AmortizationRequest
    {
        public AmortizationRequest(decimal totalPrincipal, int numberOfPeriods, decimal ratePerPeriod)
        {
            TotalPrincipal = totalPrincipal;
            NumberOfPeriods = numberOfPeriods;
            RatePerPeriod = ratePerPeriod;
        }

        public decimal TotalPrincipal { get; }
        public int NumberOfPeriods { get; }
        public decimal RatePerPeriod { get; }
    }
}
