using PaymentCalculator.Model.Financial;
using System.Linq;

namespace PaymentCalculator.Model.Amortization
{
    public class AmortizationCalculator
    {
        private readonly IFinancial _financial;

        public AmortizationCalculator(IFinancial financial)
        {
            _financial = financial;
        }

        public AmortizationResponse Calculate(AmortizationRequest request)
        {
            var monthlyPayment = _financial.Payment(request.RatePerPeriod, request.NumberOfPeriods, -request.TotalPrincipal);

            var schedule = new Period[request.NumberOfPeriods];

            for (var periodNumber = request.NumberOfPeriods; periodNumber >= 1; periodNumber--)
            {
                schedule[periodNumber - 1] = new Period(
                    periodNumber,
                    _financial.InterestPayment(request.RatePerPeriod, periodNumber, request.NumberOfPeriods, -request.TotalPrincipal),
                    _financial.PrincipalPayment(request.RatePerPeriod, periodNumber, request.NumberOfPeriods, -request.TotalPrincipal),
                    _financial.InterestPayment(request.RatePerPeriod, periodNumber + 1, request.NumberOfPeriods, -request.TotalPrincipal) / request.RatePerPeriod
                );
            }

            var totalInterestPaid = schedule.Sum(p => p.InterestPayment);
            var totalPaid = request.TotalPrincipal + totalInterestPaid;

            return new AmortizationResponse(monthlyPayment, totalInterestPaid, totalPaid, schedule, request);
        }
    }
}
