using VoidCore.Finance;

namespace PaymentCalculator.Model
{
    /// <summary>
    /// A loan customizes the standard amortization with periodic escrow and uses length of years.
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
            var amortizationRequest = new AmortizationRequest(request.TotalPrincipal, request.NumberOfPeriods, request.RatePerPeriod);

            var amortizationResponse = _amortizationCalculator.Calculate(amortizationRequest);

            return new LoanResponse(
                paymentPerPeriod: amortizationResponse.PaymentPerPeriod + request.EscrowPerPeriod,
                totalInterestPaid: amortizationResponse.TotalInterestPaid,
                totalPaid: request.AssetCost + amortizationResponse.TotalInterestPaid + request.EscrowPerPeriod * request.NumberOfPeriods,
                schedule: amortizationResponse.Schedule,
                request: request);
        }
    }
}
