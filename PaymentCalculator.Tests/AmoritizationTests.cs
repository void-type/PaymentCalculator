using PaymentCalculator.Wpf.Model;
using PaymentCalculator.Wpf.Model.Financial;
using Xunit;

namespace PaymentCalculator.Tests
{
    public class AmoritizationTests
    {
        private readonly IAmortizationCalculator _calculator = new AmortizationCalculator(new FinancialWrapper());

        [Fact]
        public void MediumMortgage()
        {
            var loan = new Loan(350000, 10000, 30, 12, .045m);

            _calculator.Calculate(loan);

            CheckLoan(1722.73m, 280182.82m, 630182.82m, 30 * 12, loan);
            CheckPeriod(452.79m, 1269.94m, 338198.98m, loan.Schedule[3]);
            CheckPeriod(1716.29m, 6.44m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        [Fact]
        public void SmallLoanMonthly()
        {
            var loan = new Loan(2000m, 0m, 5, 12, .005m);

            _calculator.Calculate(loan);

            CheckLoan(33.76m, 25.52m, 2025.52m, 5 * 12, loan);
            CheckPeriod(32.97m, 0.79m, 1868.22m, loan.Schedule[3]);
            CheckPeriod(33.74m, 0.01m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanMonthly()
        {
            var loan = new Loan(1100000m, 100000m, 40, 12, 0.20m);

            _calculator.Calculate(loan);

            CheckLoan(16672.64m, 7002867.64m, 8102867.64m, 40 * 12, loan);
            CheckPeriod(6.28m, 16666.36m, 999975.50m, loan.Schedule[3]);
            CheckPeriod(16399.32m, 273.32m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanQuarterly()
        {
            var loan = new Loan(1100000m, 100000m, 40, 4, 0.20m);

            _calculator.Calculate(loan);

            CheckLoan(50020.36m, 7003258.21m, 8103258.21m, 40 * 4, loan);
            CheckPeriod(23.57m, 49996.79m, 999912.23m, loan.Schedule[3]);
            CheckPeriod(47638.44m, 2381.92m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanYearly()
        {
            var loan = new Loan(1100000m, 100000m, 40, 1, 0.20m);

            _calculator.Calculate(loan);

            CheckLoan(200136.17m, 7005446.73m, 8105446.73m, 40 * 1, loan);
            CheckPeriod(235.30m, 199900.87m, 999269.05m, loan.Schedule[3]);
            CheckPeriod(166780.14m, 33356.03m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        [Fact]
        public void NoLoan()
        {
            var loan = new Loan(0m, 0m, 40, 12, 0.20m);

            _calculator.Calculate(loan);

            CheckLoan(0.00m, 0.00m, 0.00m, 40 * 12, loan);
            CheckPeriod(0.00m, 0.00m, 0.00m, loan.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        [Fact]
        public void PaidLoan()
        {
            var loan = new Loan(10m, 10m, 40, 12, 0.20m);

            _calculator.Calculate(loan);

            CheckLoan(0.00m, 0.00m, 10m, 40 * 12, loan);
            CheckPeriod(0.00m, 0.00m, 0.00m, loan.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        [Fact]
        public void NegativeLoan()
        {
            var loan = new Loan(0m, 20m, 40, 12, 0.20m);

            _calculator.Calculate(loan);

            Assert.Equal(0.00m, decimal.Round(loan.MonthlyPayment, 2));
            Assert.Equal(0.00m, decimal.Round(loan.TotalInterestPaid, 2));
            Assert.Equal(0.00m, decimal.Round(loan.TotalPaid, 2));
            Assert.Equal(40 * 12, loan.NumberOfPeriods);
            Assert.Equal(40 * 12, loan.Schedule.Count);

            CheckLoan(0.00m, 0.00m, 0.00m, 40 * 12, loan);
            CheckPeriod(0.00m, 0.00m, 0.00m, loan.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, loan.Schedule[loan.NumberOfPeriods - 1]);
        }

        private static void CheckLoan(decimal monthlyPayment, decimal totalInterestPaid, decimal totalPaid, int numberOfPeriods, Loan loan)
        {
            Assert.Equal(monthlyPayment, decimal.Round(loan.MonthlyPayment, 2));
            Assert.Equal(totalInterestPaid, decimal.Round(loan.TotalInterestPaid, 2));
            Assert.Equal(totalPaid, decimal.Round(loan.TotalPaid, 2));
            Assert.Equal(numberOfPeriods, loan.NumberOfPeriods);
            Assert.Equal(loan.NumberOfPeriods, loan.Schedule.Count);
        }

        private static void CheckPeriod(decimal principalPayment, decimal interestPayment, decimal balanceLeft, IPeriod actual)
        {
            Assert.Equal(principalPayment, decimal.Round(actual.PrincipalPayment, 2));
            Assert.Equal(interestPayment, decimal.Round(actual.InterestPayment, 2));
            Assert.Equal(balanceLeft, decimal.Round(actual.BalanceLeft, 2));
        }
    }
}