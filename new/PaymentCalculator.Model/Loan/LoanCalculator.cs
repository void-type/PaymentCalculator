using PaymentCalculator.Model.Amortization;
using PaymentCalculator.Model.Financial;

namespace PaymentCalculator.Model.Loan
{
    /// <summary>
    /// A loan extends the standard amortization with periodic escrow and a length of years.
    /// </summary>
    public class LoanCalculator
    {
        private readonly AmortizationCalculator _amortizationCalculator;

        public LoanCalculator(IFinancial financial)
        {
            _amortizationCalculator = new AmortizationCalculator(financial);
        }

        public LoanResponse Calculate(LoanRequest request)
        {
            var aResponse = _amortizationCalculator.Calculate(request);

            return new LoanResponse(
                aResponse.PaymentPerPeriod + request.EscrowPerPeriod,
                aResponse.TotalInterestPaid,
                request.AssetCost + aResponse.TotalInterestPaid + request.EscrowPerPeriod * request.NumberOfPeriods,
                aResponse.Schedule,
                request);
        }
    }
}
