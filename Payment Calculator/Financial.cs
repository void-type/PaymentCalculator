using System;

namespace PaymentCalculator
{
    // This is a class with some of the functionality of the Excel and VB.Net finanacial functions. This can be used when you need these functions and
    // don't have access to the legacy .Net libraries.

    public static class Financial
    {
        #region FindPayment

        public static decimal FindPayment(decimal interestRate, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            decimal payment;

            if (numberOfPeriods == 0)
            {
                payment = 0;
            }
            else if (interestRate == 0)
            {
                payment = (futureValue - presentValue) / numberOfPeriods;
            }
            else
            {
                payment = interestRate / (decimal)(Math.Pow(1 + (double)interestRate, numberOfPeriods) - 1) * -(presentValue * (decimal)Math.Pow(1 + (double)interestRate, numberOfPeriods) + futureValue);
            }

            if (paymentDueAtBeginningOfPeriod)
            {
                payment /= (1 + interestRate);
            }

            return payment;
        }

        public static double FindPayment(double interestRate, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPayment(Convert.ToDecimal(interestRate), numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        #endregion FindPayment

        #region FindInterestPayment

        public static decimal FindInterestPayment(decimal interestRate, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = FindPayment(interestRate, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FindFutureValue(interestRate, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRate;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= (1 + interestRate);
            }

            return interestPayment;
        }

        public static double FindInterestPayment(double r, int periodNumber, int numberOfPeriods, double pv, double fv = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindInterestPayment(Convert.ToDecimal(r), periodNumber, numberOfPeriods, Convert.ToDecimal(pv), Convert.ToDecimal(fv), paymentDueAtBeginningOfPeriod));
        }

        #endregion FindInterestPayment

        #region FindPrincipalPayment

        public static decimal FindPrincipalPayment(decimal interestRate, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = FindPayment(interestRate, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FindFutureValue(interestRate, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRate;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= (1 + interestRate);
            }

            return payment - interestPayment;
        }

        public static double FindPrincipalPayment(double interestRate, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPrincipalPayment(Convert.ToDecimal(interestRate), periodNumber, numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        #endregion FindPrincipalPayment

        #region FindFutureValue

        public static decimal FindFutureValue(decimal interestRate, int numberOfPeriods, decimal payment, decimal presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            decimal futureValue;
            var pow = (decimal)Math.Pow(1 + (double)interestRate, numberOfPeriods);

            if (paymentDueAtBeginningOfPeriod)
            {
                payment = payment * (1 + interestRate);
            }

            if (numberOfPeriods == 0)
            {
                futureValue = -presentValue;
            }
            else if (interestRate == 0)
            {
                futureValue = -(presentValue + (numberOfPeriods * payment));
            }
            else
            {
                futureValue = -(payment * (pow - 1) / interestRate + presentValue * pow);
            }

            return futureValue;
        }

        public static double FindFutureValue(double interestRate, int numberOfPeriods, double payment, double presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindFutureValue(Convert.ToDecimal(interestRate), numberOfPeriods, Convert.ToDecimal(payment), Convert.ToDecimal(presentValue), paymentDueAtBeginningOfPeriod));
        }

        #endregion FindFutureValue

        #region FindPresentValue

        public static decimal FindPresentValue(decimal interestRate, int numberOfPeriods, decimal payment, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var num = 1.0m;
            var pow = (decimal)Math.Pow((1 + (double)interestRate), numberOfPeriods);

            if (interestRate == 0)
            {
                return (-futureValue - (payment * numberOfPeriods));
            }

            if (paymentDueAtBeginningOfPeriod)
            {
                num = (1 + interestRate);
            }
            return (-(futureValue + ((payment * num) * ((pow - 1) / interestRate))) / pow);
        }

        public static double FindPresentValue(double interestRate, int numberOfPeriods, double payment, double presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPresentValue(Convert.ToDecimal(interestRate), numberOfPeriods, Convert.ToDecimal(payment), Convert.ToDecimal(presentValue), paymentDueAtBeginningOfPeriod));
        }

        #endregion FindPresentValue

        #region FindNetPresentValue

        public static decimal FindNetPresentValue(decimal interestRate, params decimal[] cashFlows)
        {
            var netPresentValue = 0.0m;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / (decimal)Math.Pow((1 + (double)interestRate), (i + 1));
            }

            return netPresentValue;
        }

        public static double FindNetPresentValue(double r, params double[] cashFlows)
        {
            var netPresentValue = 0.0;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / Math.Pow((1 + r), (i + 1));
            }

            return netPresentValue;
        }

        #endregion FindNetPresentValue
    }
}