using PaymentCalculator.Wpf.Model;
using PaymentCalculator.Wpf.Model.Financial;
using Xunit;

namespace PaymentCalculator.Tests
{
    public class AmoritizationTests
    {
        [Fact]
        public void MediumMortgage()
        {
            var amoritizationCalculator = new AmortizationCalculator(new FinancialWrapper());
            var loan = new Loan(350000, 10000, 30, 12, .045m);

            amoritizationCalculator.Calculate(loan);

            Assert.Equal(1722.73m, decimal.Round(loan.MonthlyPayment, 2));
            Assert.Equal(280182.82m, decimal.Round(loan.TotalInterestPaid, 2));
            Assert.Equal(630182.82m, decimal.Round(loan.TotalPaid, 2));
            Assert.Equal(30 * 12, loan.NumberOfPeriods);
            Assert.Equal(30 * 12, loan.Schedule.Count);

            Assert.Equal(452.79m, decimal.Round(loan.Schedule[3].PrincipalPayment, 2));
            Assert.Equal(1269.94m, decimal.Round(loan.Schedule[3].InterestPayment, 2));
            Assert.Equal(338198.98m, decimal.Round(loan.Schedule[3].BalanceLeft, 2));

            Assert.Equal(1716.29m, decimal.Round(loan.Schedule[359].PrincipalPayment, 2));
            Assert.Equal(6.44m, decimal.Round(loan.Schedule[359].InterestPayment, 2));
            Assert.Equal(0.00m, decimal.Round(loan.Schedule[359].BalanceLeft, 2));
        }

        [Fact]
        public void SmallLoanMonthly()
        {
            var amoritizationCalculator = new AmortizationCalculator(new FinancialWrapper());
            var loan = new Loan(2000m, 0m, 5, 12, .005m);

            amoritizationCalculator.Calculate(loan);

            Assert.Equal(33.76m, decimal.Round(loan.MonthlyPayment, 2));
            Assert.Equal(25.52m, decimal.Round(loan.TotalInterestPaid, 2));
            Assert.Equal(2025.52m, decimal.Round(loan.TotalPaid, 2));
            Assert.Equal(5 * 12, loan.NumberOfPeriods);
            Assert.Equal(5 * 12, loan.Schedule.Count);

            Assert.Equal(32.97m, decimal.Round(loan.Schedule[3].PrincipalPayment, 2));
            Assert.Equal(0.79m, decimal.Round(loan.Schedule[3].InterestPayment, 2));
            Assert.Equal(1868.22m, decimal.Round(loan.Schedule[3].BalanceLeft, 2));

            Assert.Equal(33.74m, decimal.Round(loan.Schedule[59].PrincipalPayment, 2));
            Assert.Equal(0.01m, decimal.Round(loan.Schedule[59].InterestPayment, 2));
            Assert.Equal(0.00m, decimal.Round(loan.Schedule[59].BalanceLeft, 2));
        }

        [Fact]
        public void HugeLoanMonthly()
        {
            var amoritizationCalculator = new AmortizationCalculator(new FinancialWrapper());
            var loan = new Loan(1100000m, 100000m, 40, 12, 0.20m);

            amoritizationCalculator.Calculate(loan);

            Assert.Equal(16672.64m, decimal.Round(loan.MonthlyPayment, 2));
            Assert.Equal(7002867.64m, decimal.Round(loan.TotalInterestPaid, 2));
            Assert.Equal(8102867.64m, decimal.Round(loan.TotalPaid, 2));
            Assert.Equal(40 * 12, loan.NumberOfPeriods);
            Assert.Equal(40 * 12, loan.Schedule.Count);

            Assert.Equal(6.28m, decimal.Round(loan.Schedule[3].PrincipalPayment, 2));
            Assert.Equal(16666.36m, decimal.Round(loan.Schedule[3].InterestPayment, 2));
            Assert.Equal(999975.50m, decimal.Round(loan.Schedule[3].BalanceLeft, 2));

            Assert.Equal(16399.32m, decimal.Round(loan.Schedule[479].PrincipalPayment, 2));
            Assert.Equal(273.32m, decimal.Round(loan.Schedule[479].InterestPayment, 2));
            Assert.Equal(0.00m, decimal.Round(loan.Schedule[479].BalanceLeft, 2));
        }
    }
}