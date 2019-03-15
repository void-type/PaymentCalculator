using PaymentCalculator.Model.Amortization;

namespace PaymentCalculator.Model.Loan
{
    public class LoanRequest
    {
        private readonly AmortizationRequest _innerRequest;

        public LoanRequest(decimal assetCost, decimal downPayment, decimal escrowPerPeriod, int numberOfYears, int periodsPerYear, decimal annualInterestRate)
        {
            AssetCost = assetCost;
            DownPayment = downPayment;
            EscrowPerPeriod = escrowPerPeriod;
            NumberOfYears = numberOfYears;
            PeriodsPerYear = periodsPerYear;
            AnnualInterestRate = annualInterestRate;
        }

        public decimal AssetCost { get; }
        public decimal DownPayment { get; }
        public decimal TotalPrincipal => AssetCost - DownPayment;
        public decimal EscrowPerPeriod { get; }
        public int NumberOfYears { get; }
        public int PeriodsPerYear { get; }
        public int NumberOfPeriods => NumberOfYears * PeriodsPerYear;
        public decimal AnnualInterestRate { get; }
        public decimal RatePerPeriod => AnnualInterestRate / PeriodsPerYear;
    }
}
