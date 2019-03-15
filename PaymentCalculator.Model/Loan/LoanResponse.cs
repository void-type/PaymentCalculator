using PaymentCalculator.Model.Amortization;
using System.Collections.Generic;

namespace PaymentCalculator.Model.Loan
{
    public class LoanResponse
    {
        private readonly AmortizationResponse _innerResponse;

        public LoanResponse(AmortizationResponse innerResponse, LoanRequest request)
        {
            _innerResponse = innerResponse;
            Request = request;
        }

        public decimal MonthlyPayment => _innerResponse.MonthlyPayment;
        public decimal TotalInterestPaid => _innerResponse.TotalInterestPaid;
        public decimal TotalPaid => _innerResponse.TotalPaid + Request.DownPayment;
        public decimal TotalPaidWithEscrow => TotalPaid + (Request.EscrowPerPeriod * Request.NumberOfPeriods);
        public IReadOnlyList<Period> Schedule { get; }
        public LoanRequest Request { get; }
    }
}
