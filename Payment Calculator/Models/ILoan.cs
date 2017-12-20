using System.Collections.Generic;

namespace PaymentCalculator.Models
{
    public interface ILoan
    {
        decimal AnnualInterestRate { get; }
        decimal AssetCost { get; }
        decimal DownPayment { get; }
        decimal LoanAmount { get; set; }
        decimal MonthlyPayment { get; set; }
        int NumberOfPeriods { get; set; }
        int PeriodsPerYear { get; }
        decimal RatePerPeriod { get; set; }
        List<IPeriod> Schedule { get; }
        decimal TotalInterestPaid { get; set; }
        decimal TotalPaid { get; set; }
        int Years { get; }
    }
}