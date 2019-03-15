using System.Collections.Generic;
using VoidCore.Finance;

namespace PaymentCalculator.Model
{
    public class LoanResponse
    {
        public LoanResponse(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<AmortizationPeriod> schedule, LoanRequest request)
        {
            PaymentPerPeriod = paymentPerPeriod;
            TotalInterestPaid = totalInterestPaid;
            TotalPaid = totalPaid;
            Schedule = schedule;
            Request = request;
        }

        public decimal PaymentPerPeriod { get; }
        public decimal TotalInterestPaid { get; }
        public decimal TotalPaid { get; }
        public IReadOnlyList<AmortizationPeriod> Schedule { get; }
        public LoanRequest Request { get; }
    }
}
