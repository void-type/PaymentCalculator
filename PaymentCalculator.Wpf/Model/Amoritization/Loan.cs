using System.Collections.Generic;

namespace PaymentCalculator.Wpf.Model.Amoritization
{
    /// <summary>
    /// A representation of loan properties that stay static throughout the life of the loan.
    /// </summary>
    public class Loan : ILoan
    {
        public decimal RatePerPeriod { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal AssetCost { get; }
        public decimal DownPayment { get; }
        public decimal AnnualInterestRate { get; }
        public int Years { get; }
        public int PeriodsPerYear { get; }
        public int NumberOfPeriods { get; set; }
        public List<IPeriod> Schedule { get; } = new List<IPeriod>();

        /// <summary>
        /// A loan must be initialized with assetCost, downPayment, years, periodsPerYear and
        /// </summary>
        /// <param name="assetCost"></param>
        /// <param name="downPayment"></param>
        /// <param name="years"></param>
        /// <param name="periodsPerYear"></param>
        /// <param name="annualInterestRate"></param>
        public Loan(decimal assetCost, decimal downPayment, int years, int periodsPerYear, decimal annualInterestRate)
        {
            AssetCost = assetCost;
            DownPayment = downPayment;
            Years = years;
            PeriodsPerYear = periodsPerYear;
            AnnualInterestRate = annualInterestRate;
        }
    }
}