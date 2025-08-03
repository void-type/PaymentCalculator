using PaymentCalculator.Model;
using Xunit;

namespace PaymentCalculator.Test;

public class CalculateLoanRequestValidatorTests
{
    private readonly CalculateLoan.RequestValidator _validator = new();

    [Fact]
    public void ValidRequest()
    {
        var request = new CalculateLoan.Request
        {
            AssetCost = 350000,
            DownPayment = 10000,
            EscrowPerPeriod = 0,
            NumberOfYears = 30,
            PeriodsPerYear = 12,
            AnnualInterestRate = .045m,
            PaymentModifications = []
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void InvalidWhenAssetCostNotGreaterThanDownPayment()
    {
        var request1 = new CalculateLoan.Request
        {
            AssetCost = 1,
            DownPayment = 10000,
            EscrowPerPeriod = 0,
            NumberOfYears = 30,
            PeriodsPerYear = 12,
            AnnualInterestRate = .045m,
            PaymentModifications = []
        };

        var result2 = _validator.Validate(request1);

        Assert.True(result2.IsFailed);
    }

    [Fact]
    public void InvalidWhenYearsNotGreaterThanZero()
    {
        var request1 = new CalculateLoan.Request
        {
            AssetCost = 350000,
            DownPayment = 10000,
            EscrowPerPeriod = 0,
            NumberOfYears = 0,
            PeriodsPerYear = 12,
            AnnualInterestRate = .045m,
            PaymentModifications = []
        };

        var request2 = new CalculateLoan.Request
        {
            AssetCost = 350000,
            DownPayment = 10000,
            EscrowPerPeriod = 0,
            NumberOfYears = -1,
            PeriodsPerYear = 12,
            AnnualInterestRate = .045m,
            PaymentModifications = []
        };

        var result1 = _validator.Validate(request1);
        var result2 = _validator.Validate(request2);

        Assert.True(result1.IsFailed);
        Assert.True(result2.IsFailed);
    }

    [Fact]
    public void InvalidWhenPeriodsPerYearNotGreaterThanZero()
    {
        var request1 = new CalculateLoan.Request
        {
            AssetCost = 350000,
            DownPayment = 10000,
            EscrowPerPeriod = 0,
            NumberOfYears = 30,
            PeriodsPerYear = 0,
            AnnualInterestRate = .045m,
            PaymentModifications = []
        };

        var request2 = new CalculateLoan.Request
        {
            AssetCost = 350000,
            DownPayment = 10000,
            EscrowPerPeriod = 0,
            NumberOfYears = 30,
            PeriodsPerYear = -1,
            AnnualInterestRate = .045m,
            PaymentModifications = []
        };

        var result1 = _validator.Validate(request1);
        var result2 = _validator.Validate(request2);

        Assert.True(result1.IsFailed);
        Assert.True(result2.IsFailed);
    }
}
