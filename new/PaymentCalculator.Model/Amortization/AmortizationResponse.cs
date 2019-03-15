using System.Collections.Generic;

namespace PaymentCalculator.Model.Amortization
{
    public class AmortizationResponse
    {
        public AmortizationResponse(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<Period> schedule, IAmortizationRequest request)
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
        public IReadOnlyList<Period> Schedule { get; }
        public IAmortizationRequest Request { get; }
    }
}
