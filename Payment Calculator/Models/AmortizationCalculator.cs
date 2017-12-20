namespace PaymentCalculator.Models
{
    /// <summary>
    /// Calculates statistics and amoritization schedule of the loan.
    /// </summary>
    public class AmortizationCalculator : IAmortizationCalculator
    {
        private readonly IFinancial _financial;

        public AmortizationCalculator(IFinancial financial)
        {
            _financial = financial;
        }

        /// <summary>
        /// Pass a view model with minimum loan information (AssetCost, Down, Years, Periods per year, and APR) to calculate the remaining stats and schedule.
        /// </summary>
        /// <param name="loan"></param>
        public void Calculate(ILoan loan)
        {
            CalculateLoan(loan);
            CalculateSchedule(loan);
        }

        /// <summary>
        /// Builds an amoritization table with columns for payment number, interest of the payment, principle of the payment, and balance left of the mortgage.
        /// </summary>
        /// <param name="loan">An amoritization viewmodel to hold loan calculation state.</param>
        /// <returns></returns>
        private void CalculateSchedule(ILoan loan)
        {
            loan.TotalInterestPaid = 0;

            for (var periodNumber = 1; periodNumber <= loan.NumberOfPeriods; periodNumber++)
            {
                var row = new Period
                {
                    PeriodNumber = periodNumber,
                    InterestPayment = _financial.FindInterestPayment(loan.RatePerPeriod, periodNumber, loan.NumberOfPeriods, -loan.LoanAmount),
                    PrincipalPayment = _financial.FindPrincipalPayment(loan.RatePerPeriod, periodNumber, loan.NumberOfPeriods, -loan.LoanAmount),
                    BalanceLeft = _financial.FindInterestPayment(loan.RatePerPeriod, periodNumber + 1, loan.NumberOfPeriods, -loan.LoanAmount) / loan.RatePerPeriod,
                };

                loan.Schedule.Add(row);

                loan.TotalInterestPaid += row.InterestPayment;
                loan.TotalPaid = loan.TotalInterestPaid + loan.AssetCost;
            }
        }

        /// <summary>
        /// Calculates the missing loan information so a schedule can be created.
        /// </summary>
        /// <param name="loan"></param>
        /// <returns></returns>
        private void CalculateLoan(ILoan loan)
        {
            loan.LoanAmount = loan.AssetCost - loan.DownPayment;
            loan.NumberOfPeriods = loan.Years * loan.PeriodsPerYear;
            loan.RatePerPeriod = loan.AnnualInterestRate / loan.PeriodsPerYear;
            loan.MonthlyPayment = _financial.FindPayment(loan.RatePerPeriod, loan.NumberOfPeriods, -loan.LoanAmount);
        }
    }
}