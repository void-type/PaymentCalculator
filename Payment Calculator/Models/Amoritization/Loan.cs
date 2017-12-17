namespace PaymentCalculator.Models.Amoritization
{
    /// <summary>
    /// A representation of loan properties that stay static throughout the life of the loan.
    /// </summary>
    public class Loan
    {
        public decimal RatePerPeriod { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal AssetCost { get; set; }
        public decimal DownPayment { get; set; }
        public decimal InterestRate { get; set; }
        public int Years { get; set; }
        public int PeriodsPerYear { get; set; }
        public int NumberOfPeriods { get; set; }
    }
}