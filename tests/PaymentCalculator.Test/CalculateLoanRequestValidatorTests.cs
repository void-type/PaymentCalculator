using PaymentCalculator.Model;
using Xunit;

namespace PaymentCalculator.Test
{
    public class CalculateLoanRequestValidatorTests
    {
        private readonly CalculateLoan.RequestValidator _validator = new CalculateLoan.RequestValidator();

        [Fact]
        public void ValidRequest()
        {
            var request = new CalculateLoan.Request(350000, 10000, 0, 30, 12, .045m);

            var result = _validator.Validate(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void InvalidWhenAssetCostNotGreaterThanDownPayment()
        {
            var request1 = new CalculateLoan.Request(10000, 10000, 0, 30, 12, .045m);
            var request2 = new CalculateLoan.Request(1, 10000, 0, 30, 12, .045m);

            var result1 = _validator.Validate(request1);
            var result2 = _validator.Validate(request2);

            Assert.True(result1.IsFailed);
            Assert.True(result2.IsFailed);
        }

        [Fact]
        public void InvalidWhenYearsNotGreaterThanZero()
        {
            var request1 = new CalculateLoan.Request(350000, 10000, 0, 0, 12, .045m);
            var request2 = new CalculateLoan.Request(350000, 10000, 0, -1, 12, .045m);

            var result1 = _validator.Validate(request1);
            var result2 = _validator.Validate(request2);

            Assert.True(result1.IsFailed);
            Assert.True(result2.IsFailed);
        }

        [Fact]
        public void InvalidWhenPeriodsPerYearNotGreaterThanZero()
        {
            var request1 = new CalculateLoan.Request(350000, 10000, 0, 30, 0, .045m);
            var request2 = new CalculateLoan.Request(350000, 10000, 0, 30, -1, .045m);

            var result1 = _validator.Validate(request1);
            var result2 = _validator.Validate(request2);

            Assert.True(result1.IsFailed);
            Assert.True(result2.IsFailed);
        }
    }
}
