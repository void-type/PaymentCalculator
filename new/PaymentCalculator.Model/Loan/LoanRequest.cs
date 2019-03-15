using PaymentCalculator.Model.Amortization;

namespace PaymentCalculator.Model.Loan
{
    public class LoanRequest : IAmortizationRequest
    {
        public LoanRequest(decimal assetCost, decimal downPayment, decimal escrowPerPeriod, int numberOfYears, int periodsPerYear, decimal annualInterestRate)
        {
            AssetCost = assetCost;
            DownPayment = downPayment;
            EscrowPerPeriod = escrowPerPeriod;
            NumberOfYears = numberOfYears;
            PeriodsPerYear = periodsPerYear;
            AnnualInterestRate = annualInterestRate;
            TotalPrincipal = assetCost - downPayment;
            NumberOfPeriods = numberOfYears * periodsPerYear;
            RatePerPeriod = annualInterestRate / periodsPerYear;
        }

        public decimal AssetCost { get; }
        public decimal DownPayment { get; }
        public decimal TotalPrincipal { get; }
        public decimal EscrowPerPeriod { get; }
        public int NumberOfYears { get; }
        public int PeriodsPerYear { get; }
        public int NumberOfPeriods { get; }
        public decimal AnnualInterestRate { get; }
        public decimal RatePerPeriod { get; }
    }
}
