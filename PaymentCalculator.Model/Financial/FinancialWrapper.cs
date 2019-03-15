namespace PaymentCalculator.Model.Financial
{
    public class FinancialWrapper : IFinancial
    {
        public decimal FutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FutureValue(interestRatePerPeriod, numberOfPeriods, payment, presentValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal InterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue,
            decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.InterestPayment(interestRatePerPeriod, periodNumber, numberOfPeriods, presentValue, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal NetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows)
        {
            return Financial.NetPresentValue(interestRatePerPeriod, cashFlows);
        }

        public decimal Payment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.Payment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal PresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.PresentValue(interestRatePerPeriod, numberOfPeriods, payment, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal PrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue,
            decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.PrincipalPayment(interestRatePerPeriod, periodNumber, numberOfPeriods, presentValue,
                futureValue, paymentDueAtBeginningOfPeriod);
        }
    }
}
