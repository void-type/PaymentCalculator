using System;
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
                    periodNumber: periodNumber,
                    interestPayment: _financial.InterestPayment(ratePerPeriod, periodNumber, numberOfPeriods, -totalPrincipal),
                    principalPayment : _financial.PrincipalPayment(ratePerPeriod, periodNumber, numberOfPeriods, -totalPrincipal),
                    balanceLeft : _financial.InterestPayment(ratePerPeriod, periodNumber + 1, numberOfPeriods, -totalPrincipal) / ratePerPeriod
                );
            });

            var paymentPerPeriod = _financial.Payment(ratePerPeriod, numberOfPeriods, -totalPrincipal);
            var totalInterestPaid = schedule.Sum(p => p.InterestPayment);
            var totalPaid = totalPrincipal + totalInterestPaid;

            return new AmortizationResponse(paymentPerPeriod, totalInterestPaid, totalPaid, schedule, request);
        }

        public AmortizationResponse CalculateExperimental(IAmortizationRequest request, params PaymentDeviation[] deviations)
        {
            var ratePerPeriod = request.RatePerPeriod;
            var numberOfPeriods = request.NumberOfPeriods;
            var totalPrincipal = request.TotalPrincipal;
            var paymentPerPeriod = _financial.Payment(ratePerPeriod, numberOfPeriods, -totalPrincipal);

            var schedule = new AmortizationPeriod[numberOfPeriods];

            Parallel.For(1, numberOfPeriods + 1, periodNumber =>
            {
                var deviationsToApply = deviations
                    .Where(d => d.PeriodNumber < periodNumber)
                    .Sum(d => d.Amount);

                var adjustedPrincipal = totalPrincipal - deviationsToApply;

                var adjustedNumberOfPeriods = (int) Math.Ceiling(_financial.NumberOfPeriods(ratePerPeriod, paymentPerPeriod, -adjustedPrincipal));

                schedule[periodNumber - 1] = new AmortizationPeriod(
                    periodNumber: periodNumber,
                    interestPayment: _financial.InterestPayment(ratePerPeriod, periodNumber, adjustedNumberOfPeriods, -adjustedPrincipal),
                    principalPayment : _financial.PrincipalPayment(ratePerPeriod, periodNumber, adjustedNumberOfPeriods, -adjustedPrincipal),
                    balanceLeft : _financial.InterestPayment(ratePerPeriod, periodNumber + 1, adjustedNumberOfPeriods, -adjustedPrincipal) / ratePerPeriod
                );
            });

            var totalInterestPaid = schedule.Sum(p => p.InterestPayment);
            var totalPaid = totalPrincipal + totalInterestPaid;

            return new AmortizationResponse(paymentPerPeriod, totalInterestPaid, totalPaid, schedule, request);
        }
    }
}
