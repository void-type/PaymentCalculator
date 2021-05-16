using PaymentCalculator.Model;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Finance;
using VoidCore.Model.Functional;
using Xunit;

namespace PaymentCalculator.Test
{
    public class CalculateLoanHandlerTests
    {
        private readonly CalculateLoan.Handler _calculator = new(new AmortizationCalculator(new Financial()));

        [Fact]
        public async Task MediumMortgage()
        {
            var request = new CalculateLoan.Request(350000, 10000, 0, 30, 12, .045m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(1722.73m, 280182.82m, 630182.82m, 30 * 12, response);
            CheckPeriod(452.79m, 1269.94m, 338198.98m, response.Schedule[3]);
            CheckPeriod(1716.29m, 6.44m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task SmallLoanMonthly()
        {
            var request = new CalculateLoan.Request(2000m, 0m, 15, 5, 12, .005m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(48.76m, 25.52m, 2925.52m, 5 * 12, response);
            CheckPeriod(32.97m, 0.79m, 1868.22m, response.Schedule[3]);
            CheckPeriod(33.74m, 0.01m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task HugeLoanMonthly()
        {
            var request = new CalculateLoan.Request(1100000m, 100000m, 0, 40, 12, 0.20m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(16672.64m, 7002867.64m, 8102867.64m, 40 * 12, response);
            CheckPeriod(6.28m, 16666.36m, 999975.50m, response.Schedule[3]);
            CheckPeriod(16399.32m, 273.32m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task HugeLoanQuarterly()
        {
            var request = new CalculateLoan.Request(1100000m, 100000m, 0, 40, 4, 0.20m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(50020.36m, 7003258.21m, 8103258.21m, 40 * 4, response);
            CheckPeriod(23.57m, 49996.79m, 999912.23m, response.Schedule[3]);
            CheckPeriod(47638.44m, 2381.92m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task HugeLoanYearly()
        {
            var request = new CalculateLoan.Request(1100000m, 100000m, 0, 40, 1, 0.20m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(200136.17m, 7005446.73m, 8105446.73m, 40 * 1, response);
            CheckPeriod(235.30m, 199900.87m, 999269.05m, response.Schedule[3]);
            CheckPeriod(166780.14m, 33356.03m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task NoLoan()
        {
            var request = new CalculateLoan.Request(0m, 0m, 0, 40, 12, 0.20m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(0.00m, 0.00m, 0.00m, 40 * 12, response);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task PaidLoan()
        {
            var request = new CalculateLoan.Request(10m, 10m, 0, 40, 12, 0.20m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(0.00m, 0.00m, 10m, 40 * 12, response);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task LongLoanMonthly()
        {
            var request = new CalculateLoan.Request(1100m, 10m, 0, 200, 12, 0.05m);

            var response = await _calculator.Handle(request).MapAsync(r => r.Value);

            CheckLoan(4.54m, 9810.51m, 10910.51m, 200 * 12, response);
            CheckPeriod(0.00m, 4.54m, 1090.00m, response.Schedule[3]);
            CheckPeriod(4.52m, 0.02m, 0.00m, response.Schedule[response.Schedule.Count - 1]);
        }

        [Fact]
        public async Task TooLargeLoanReturnsFailure()
        {
            var request = new CalculateLoan.Request(1100m, 10m, 0, 20000000, 12, 0.5m);

            var result = await _calculator.Handle(request);

            Assert.True(result.IsFailed);
            Assert.IsType<LoanOverflowFailure>(result.Failures.Single());
        }

        private static void CheckLoan(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, int numberOfPeriods, CalculateLoan.Response response)
        {
            Assert.Equal(paymentPerPeriod, decimal.Round(response.PaymentPerPeriod, 2));
            Assert.Equal(totalInterestPaid, decimal.Round(response.TotalInterestPaid, 2));
            Assert.Equal(totalPaid, decimal.Round(response.TotalPaid, 2));
            Assert.Equal(numberOfPeriods, response.Schedule.Count);
        }

        private static void CheckPeriod(decimal principalPayment, decimal interestPayment, decimal balanceLeft, AmortizationPeriod actual)
        {
            Assert.Equal(principalPayment, decimal.Round(actual.PrincipalPayment, 2));
            Assert.Equal(interestPayment, decimal.Round(actual.InterestPayment, 2));
            Assert.Equal(balanceLeft, decimal.Round(actual.BalanceLeft, 2));
        }
    }
}
