using PaymentCalculator.Model.Amortization;
using PaymentCalculator.Model.Financial;

namespace PaymentCalculator.Model.Loan
{
    public class LoanCalculator
    {
        public LoanResponse Calculate(LoanRequest request)
        {
            var amortizationRequest = new AmortizationRequest(request.TotalPrincipal, request.NumberOfPeriods, request.RatePerPeriod);

            var amortizationResponse = new AmortizationCalculator(new FinancialWrapper()).Calculate(amortizationRequest);

            return new LoanResponse(amortizationResponse, request);
        }
    }
}
