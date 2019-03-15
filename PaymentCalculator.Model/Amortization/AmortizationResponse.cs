using System.Collections.Generic;

namespace PaymentCalculator.Model.Amortization
{
    public class AmortizationResponse
    {
        public AmortizationResponse(decimal monthlyPayment, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<Period> schedule, AmortizationRequest request)
        {
            MonthlyPayment = monthlyPayment;
            TotalInterestPaid = totalInterestPaid;
            TotalPaid = totalPaid;
            Request = request;
        }

        public decimal MonthlyPayment { get; }
        public decimal TotalInterestPaid { get; }
        public decimal TotalPaid { get; }
        public IReadOnlyList<Period> Schedule { get; }
        public AmortizationRequest Request { get; }
    }
}
