using System;

namespace VoidCore.Finance
{
    public sealed class AmortizationRequest
    {
        public AmortizationRequest(decimal totalPrincipal, int numberOfPeriods, decimal ratePerPeriod)
        {
            if (totalPrincipal < 0)
            {
                throw new ArgumentException("Cannot be less than 0.", nameof(totalPrincipal));
            }

            if (numberOfPeriods < 1)
            {
                throw new ArgumentException("Cannot be less than 1.", nameof(numberOfPeriods));
            }

            if (ratePerPeriod < 0)
            {
                throw new ArgumentException("Cannot be less than 0.", nameof(ratePerPeriod));
            }

            TotalPrincipal = totalPrincipal;
            NumberOfPeriods = numberOfPeriods;
            RatePerPeriod = ratePerPeriod;
        }

        public decimal TotalPrincipal { get; }
        public int NumberOfPeriods { get; }
        public decimal RatePerPeriod { get; }
    }
}
