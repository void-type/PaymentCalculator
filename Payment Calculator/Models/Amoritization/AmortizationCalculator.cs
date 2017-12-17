namespace PaymentCalculator.Models.Amoritization
{
    public static class AmortizationCalculator
    {
        /// <summary>
        /// Pass a view model with minimum loan information (AssetCost, Down, Years, Periods per year, and APR) to calculate the remaining stats and schedule.
        /// </summary>
        /// <param name="viewModel"></param>
        public static void Calculate(AmoritizationViewModel viewModel)
        {
            CalculateLoan(viewModel);
            CalculateSchedule(viewModel);
        }

        /// <summary>
        /// Builds an amoritization table with columns for payment number, interest of the payment, principle of the payment, and balance left of the mortgage.
        /// </summary>
        /// <param name="viewModel">An amoritization viewmodel to hold loan calculation state.</param>
        /// <returns></returns>
        private static void CalculateSchedule(AmoritizationViewModel viewModel)
        {
            var loan = viewModel.Loan;
            loan.TotalInterestPaid = 0;

            for (var periodNumber = 1; periodNumber <= viewModel.Loan.NumberOfPeriods; periodNumber++)
            {
                var row = new Period
                {
                    PeriodNumber = periodNumber,
                    InterestPayment = Financial.FindInterestPayment(loan.RatePerPeriod, periodNumber, loan.NumberOfPeriods, -loan.LoanAmount),
                    PrincipalPayment = Financial.FindPrincipalPayment(loan.RatePerPeriod, periodNumber, loan.NumberOfPeriods, -loan.LoanAmount),
                    BalanceLeft = Financial.FindInterestPayment(loan.RatePerPeriod, periodNumber + 1, loan.NumberOfPeriods, -loan.LoanAmount) / loan.RatePerPeriod,
                };

                viewModel.Schedule.Add(row);

                loan.TotalInterestPaid += row.InterestPayment;
                loan.TotalPaid = loan.TotalInterestPaid + loan.AssetCost;
            }
        }

        /// <summary>
        /// Calculates the missing loan information so a schedule can be created.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static void CalculateLoan(AmoritizationViewModel viewModel)
        {
            var loan = viewModel.Loan;
            loan.LoanAmount = loan.AssetCost - loan.DownPayment;
            loan.NumberOfPeriods = loan.Years * loan.PeriodsPerYear;
            loan.RatePerPeriod = loan.InterestRate / loan.PeriodsPerYear;
            loan.MonthlyPayment = Financial.FindPayment(loan.RatePerPeriod, loan.NumberOfPeriods, -loan.LoanAmount);
        }
    }
}