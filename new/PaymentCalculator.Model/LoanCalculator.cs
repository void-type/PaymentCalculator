using VoidCore.Finance;

namespace PaymentCalculator.Model
{
    /// <summary>
    /// A loan customizes the standard amortization with periodic escrow and a length of years.
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
            var amortization = _amortizationCalculator.Calculate(request);

            return new LoanResponse(
                paymentPerPeriod: amortization.PaymentPerPeriod + request.EscrowPerPeriod,
                totalInterestPaid: amortization.TotalInterestPaid,
                totalPaid: request.AssetCost + amortization.TotalInterestPaid + request.EscrowPerPeriod * request.NumberOfPeriods,
                schedule: amortization.Schedule,
                request: request);
        }
    }
}
