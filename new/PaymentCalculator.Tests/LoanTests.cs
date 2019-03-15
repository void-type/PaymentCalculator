using PaymentCalculator.Model;
using VoidCore.Finance;
using Xunit;

namespace PaymentCalculator.Tests
{
    public class LoanTests
    {
        private readonly LoanCalculator _calculator = new LoanCalculator(new Financial());

        [Fact]
        public void MediumMortgage()
        {
            var request = new LoanRequest(350000, 10000, 0, 30, 12, .045m);

            var response = _calculator.Calculate(request);

            CheckLoan(1722.73m, 280182.82m, 630182.82m, 30 * 12, response);
            CheckPeriod(452.79m, 1269.94m, 338198.98m, response.Schedule[3]);
            CheckPeriod(1716.29m, 6.44m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void SmallLoanMonthly()
        {
            var request = new LoanRequest(2000m, 0m, 0, 5, 12, .005m);

            var response = _calculator.Calculate(request);

            CheckLoan(33.76m, 25.52m, 2025.52m, 5 * 12, response);
            CheckPeriod(32.97m, 0.79m, 1868.22m, response.Schedule[3]);
            CheckPeriod(33.74m, 0.01m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanMonthly()
        {
            var request = new LoanRequest(1100000m, 100000m, 0, 40, 12, 0.20m);

            var response = _calculator.Calculate(request);

            CheckLoan(16672.64m, 7002867.64m, 8102867.64m, 40 * 12, response);
            CheckPeriod(6.28m, 16666.36m, 999975.50m, response.Schedule[3]);
            CheckPeriod(16399.32m, 273.32m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanQuarterly()
        {
            var request = new LoanRequest(1100000m, 100000m, 0, 40, 4, 0.20m);

            var response = _calculator.Calculate(request);

            CheckLoan(50020.36m, 7003258.21m, 8103258.21m, 40 * 4, response);
            CheckPeriod(23.57m, 49996.79m, 999912.23m, response.Schedule[3]);
            CheckPeriod(47638.44m, 2381.92m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanYearly()
        {
            var request = new LoanRequest(1100000m, 100000m, 0, 40, 1, 0.20m);

            var response = _calculator.Calculate(request);

            CheckLoan(200136.17m, 7005446.73m, 8105446.73m, 40 * 1, response);
            CheckPeriod(235.30m, 199900.87m, 999269.05m, response.Schedule[3]);
            CheckPeriod(166780.14m, 33356.03m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void NoLoan()
        {
            var request = new LoanRequest(0m, 0m, 0, 40, 12, 0.20m);

            var response = _calculator.Calculate(request);

            CheckLoan(0.00m, 0.00m, 0.00m, 40 * 12, response);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void PaidLoan()
        {
            var request = new LoanRequest(10m, 10m, 0, 40, 12, 0.20m);

            var response = _calculator.Calculate(request);

            CheckLoan(0.00m, 0.00m, 10m, 40 * 12, response);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void LongLoanMonthly()
        {
            var request = new LoanRequest(1100m, 10m, 0, 200, 12, 0.05m);

            var response = _calculator.Calculate(request);

            CheckLoan(4.54m, 9810.51m, 10910.51m, 200 * 12, response);
            CheckPeriod(0.00m, 4.54m, 1090.00m, response.Schedule[3]);
            CheckPeriod(4.52m, 0.02m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        // [Fact]
        // public void PerformanceTest()
        // {
        //     var watch = new System.Diagnostics.Stopwatch();
        //     var request = new LoanRequest(100m, 1m, 0, 1000000, 12, 0.000000000001m);
        //     watch.Start();
        //     var response = _calculator.Calculate(request);
        //     watch.Stop();
        //     var numberOfPeriods = response.Request.NumberOfPeriods;
        //     var time = watch.ElapsedMilliseconds;
        // }

        private static void CheckLoan(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, int numberOfPeriods, LoanResponse loan)
        {
            Assert.Equal(paymentPerPeriod, decimal.Round(loan.PaymentPerPeriod, 2));
            Assert.Equal(totalInterestPaid, decimal.Round(loan.TotalInterestPaid, 2));
            Assert.Equal(totalPaid, decimal.Round(loan.TotalPaid, 2));
            Assert.Equal(numberOfPeriods, loan.Request.NumberOfPeriods);
            Assert.Equal(loan.Request.NumberOfPeriods, loan.Schedule.Count);
        }

        private static void CheckPeriod(decimal principalPayment, decimal interestPayment, decimal balanceLeft, AmortizationPeriod actual)
        {
            Assert.Equal(principalPayment, decimal.Round(actual.PrincipalPayment, 2));
            Assert.Equal(interestPayment, decimal.Round(actual.InterestPayment, 2));
            Assert.Equal(balanceLeft, decimal.Round(actual.BalanceLeft, 2));
        }
    }
}
