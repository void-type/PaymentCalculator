using System;

namespace PaymentCalculator
{
    /// <summary>
    /// This class has the functionality of the VB.Net and Excel Financial functions, which specialize in time-value of money.
    /// </summary>
    public static class Financial
    {
        /// <summary>
        /// Finds the future value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the future value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindFutureValue(double interestRate, int numberOfPeriods, double payment, double presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindFutureValue(Convert.ToDecimal(interestRate), numberOfPeriods, Convert.ToDecimal(payment), Convert.ToDecimal(presentValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the amount of interest paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the amount of interest paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindInterestPayment(double interestRate, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindInterestPayment(Convert.ToDecimal(interestRate), periodNumber, numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
        /// <returns></returns>
        public static decimal FindNetPresentValue(decimal interestRate, params decimal[] cashFlows)
        {
            var netPresentValue = 0.0m;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / (decimal)Math.Pow((1 + (double)interestRate), (i + 1));
            }

            return netPresentValue;
        }

        /// <summary>
        /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
        /// <returns></returns>
        public static double FindNetPresentValue(double interestRate, params double[] cashFlows)
        {
            var netPresentValue = 0.0;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / Math.Pow((1 + interestRate), (i + 1));
            }

            return netPresentValue;
        }

        /// <summary>
        /// Finds the payment amount per period for an an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the payment amount per period for an an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindPayment(double interestRate, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPayment(Convert.ToDecimal(interestRate), numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindPresentValue(double interestRate, int numberOfPeriods, double payment, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPresentValue(Convert.ToDecimal(interestRate), numberOfPeriods, Convert.ToDecimal(payment), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the amount of principal paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the amount of principal paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRate">The interest rate per period. Note: use APR divded by number of periods in a year.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindPrincipalPayment(double interestRate, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPrincipalPayment(Convert.ToDecimal(interestRate), periodNumber, numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }
    }
}