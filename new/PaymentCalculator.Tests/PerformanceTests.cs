using PaymentCalculator.Model;
using VoidCore.Finance;
using Xunit;

namespace PaymentCalculator.Tests
{
    public class PerformanceTests
    {
        private readonly LoanCalculator _calculator = new LoanCalculator(new Financial());

        // [Fact]
        // public void PerformanceTest()
        // {
        //     // Calculate a million year mortgage, may cause unchecked overflow
        //     var request = new LoanRequest(2000m, 1m, 0, 1000000, 12, 0.000001m);
        //     var response = _calculator.Calculate(request);

        //     Assert.Equal(1000000 * 12, response.Request.NumberOfPeriods);
        // }

        // [Fact]
        // public void CpuBurner()
        // {
        //     // Calculate a 5 million year mortgage, may cause unchecked overflow and excessive cpu heat
        //     var request = new LoanRequest(2000m, 1m, 0, 5000000, 12, 0.000001m);
        //     var response = _calculator.Calculate(request);

        //     Assert.Equal(5000000 * 12, response.Request.NumberOfPeriods);
        // }
    }
}
