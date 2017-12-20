namespace PaymentCalculator.Wpf.Model.Financial
{
    public class FinancialWrapper : IFinancial
    {
        public decimal FindFutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindFutureValue(interestRatePerPeriod, numberOfPeriods, payment, presentValue,
                paymentDueAtBeginningOfPeriod);
        }

        public double FindFutureValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double presentValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindFutureValue(interestRatePerPeriod, numberOfPeriods, payment, presentValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal FindInterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue,
            decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindInterestPayment(interestRatePerPeriod, periodNumber, numberOfPeriods, presentValue, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public double FindInterestPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue,
            double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindInterestPayment(interestRatePerPeriod, periodNumber, numberOfPeriods, presentValue, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal FindNetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows)
        {
            return Financial.FindNetPresentValue(interestRatePerPeriod, cashFlows);
        }

        public double FindNetPresentValue(double interestRatePerPeriod, params double[] cashFlows)
        {
            return Financial.FindNetPresentValue(interestRatePerPeriod, cashFlows);
        }

        public decimal FindPayment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindPayment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public double FindPayment(double interestRatePerPeriod, int numberOfPeriods, double presentValue, double futureValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindPayment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal FindPresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindPresentValue(interestRatePerPeriod, numberOfPeriods, payment, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public double FindPresentValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double futureValue = 0,
            bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindPresentValue(interestRatePerPeriod, numberOfPeriods, payment, futureValue,
                paymentDueAtBeginningOfPeriod);
        }

        public decimal FindPrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue,
            decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindPrincipalPayment(interestRatePerPeriod, periodNumber, numberOfPeriods, presentValue,
                futureValue, paymentDueAtBeginningOfPeriod);
        }

        public double FindPrincipalPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue,
            double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Financial.FindPrincipalPayment(interestRatePerPeriod, periodNumber, numberOfPeriods, presentValue,
                futureValue, paymentDueAtBeginningOfPeriod);
        }
    }
}