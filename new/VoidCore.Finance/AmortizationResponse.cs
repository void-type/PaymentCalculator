using System.Collections.Generic;

namespace VoidCore.Finance
{
    public class AmortizationResponse
    {
        internal AmortizationResponse(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<AmortizationPeriod> schedule, AmortizationRequest request)
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
        public AmortizationRequest Request { get; }
    }
}
