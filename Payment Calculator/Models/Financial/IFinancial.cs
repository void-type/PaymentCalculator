namespace PaymentCalculator.Models
{
    public interface IFinancial
    {
        decimal FindFutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        double FindFutureValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double presentValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        decimal FindInterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        double FindInterestPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        decimal FindNetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows);

        double FindNetPresentValue(double interestRatePerPeriod, params double[] cashFlows);

        decimal FindPayment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        double FindPayment(double interestRatePerPeriod, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        decimal FindPresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        double FindPresentValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        decimal FindPrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        double FindPrincipalPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);
    }
}