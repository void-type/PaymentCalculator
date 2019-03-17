using System.Linq;
using System.Threading.Tasks;

namespace VoidCore.Finance
{
    public class AmortizationCalculator
    {
        private readonly IFinancial _financial;

        public AmortizationCalculator(IFinancial financial)
        {
            _financial = financial;
        }

        public AmortizationResponse Calculate(IAmortizationRequest request)
        {
            var ratePerPeriod = request.RatePerPeriod;
            var numberOfPeriods = request.NumberOfPeriods;
            var totalPrincipal = request.TotalPrincipal;

            var schedule = new AmortizationPeriod[numberOfPeriods];

            Parallel.For(1, numberOfPeriods + 1, periodNumber =>
            {
                schedule[periodNumber - 1] = new AmortizationPeriod(
                    periodNumber,
                    _financial.InterestPayment(ratePerPeriod, periodNumber, numberOfPeriods, -totalPrincipal),
                    _financial.PrincipalPayment(ratePerPeriod, periodNumber, numberOfPeriods, -totalPrincipal),
                    _financial.InterestPayment(ratePerPeriod, periodNumber + 1, numberOfPeriods, -totalPrincipal) / ratePerPeriod
                );
            });

            var paymentPerPeriod = _financial.Payment(ratePerPeriod, numberOfPeriods, -totalPrincipal);
            var totalInterestPaid = schedule.Sum(p => p.InterestPayment);
            var totalPaid = totalPrincipal + totalInterestPaid;

            return new AmortizationResponse(paymentPerPeriod, totalInterestPaid, totalPaid, schedule, request);
        }
    }
}
