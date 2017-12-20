using System;

namespace PaymentCalculator.Wpf.Model.Financial
{
    /// <summary>
    /// This class has the functionality of the VB.Net and Excel Financial functions, which specialize in time-value of money.
    /// </summary>
    public static class Financial
    {
        /// <summary>
        /// Finds the future value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static decimal FindFutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            decimal futureValue;
            var pow = (decimal)Math.Pow(1 + (double)interestRatePerPeriod, numberOfPeriods);

            if (paymentDueAtBeginningOfPeriod)
            {
                payment = payment * (1 + interestRatePerPeriod);
            }

            if (numberOfPeriods == 0)
            {
                futureValue = -presentValue;
            }
            else if (interestRatePerPeriod == 0)
            {
                futureValue = -(presentValue + (numberOfPeriods * payment));
            }
            else
            {
                futureValue = -(payment * (pow - 1) / interestRatePerPeriod + presentValue * pow);
            }

            return futureValue;
        }

        /// <summary>
        /// Finds the future value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindFutureValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindFutureValue(Convert.ToDecimal(interestRatePerPeriod), numberOfPeriods, Convert.ToDecimal(payment), Convert.ToDecimal(presentValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the amount of interest paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static decimal FindInterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = FindPayment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FindFutureValue(interestRatePerPeriod, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRatePerPeriod;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= (1 + interestRatePerPeriod);
            }

            return interestPayment;
        }

        /// <summary>
        /// Finds the amount of interest paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindInterestPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindInterestPayment(Convert.ToDecimal(interestRatePerPeriod), periodNumber, numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
        /// <returns></returns>
        public static decimal FindNetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows)
        {
            var netPresentValue = 0.0m;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / (decimal)Math.Pow((1 + (double)interestRatePerPeriod), (i + 1));
            }

            return netPresentValue;
        }

        /// <summary>
        /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
        /// <returns></returns>
        public static double FindNetPresentValue(double interestRatePerPeriod, params double[] cashFlows)
        {
            var netPresentValue = 0.0;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / Math.Pow((1 + interestRatePerPeriod), (i + 1));
            }

            return netPresentValue;
        }

        /// <summary>
        /// Finds the payment amount per period for an an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static decimal FindPayment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            decimal payment;

            if (numberOfPeriods == 0)
            {
                payment = 0;
            }
            else if (interestRatePerPeriod == 0)
            {
                payment = (futureValue - presentValue) / numberOfPeriods;
            }
            else
            {
                payment = interestRatePerPeriod / (decimal)(Math.Pow(1 + (double)interestRatePerPeriod, numberOfPeriods) - 1) * -(presentValue * (decimal)Math.Pow(1 + (double)interestRatePerPeriod, numberOfPeriods) + futureValue);
            }

            if (paymentDueAtBeginningOfPeriod)
            {
                payment /= (1 + interestRatePerPeriod);
            }

            return payment;
        }

        /// <summary>
        /// Finds the payment amount per period for an an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindPayment(double interestRatePerPeriod, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPayment(Convert.ToDecimal(interestRatePerPeriod), numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static decimal FindPresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var num = 1.0m;
            var pow = (decimal)Math.Pow((1 + (double)interestRatePerPeriod), numberOfPeriods);

            if (interestRatePerPeriod == 0)
            {
                return (-futureValue - (payment * numberOfPeriods));
            }

            if (paymentDueAtBeginningOfPeriod)
            {
                num = (1 + interestRatePerPeriod);
            }
            return (-(futureValue + ((payment * num) * ((pow - 1) / interestRatePerPeriod))) / pow);
        }

        /// <summary>
        /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindPresentValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPresentValue(Convert.ToDecimal(interestRatePerPeriod), numberOfPeriods, Convert.ToDecimal(payment), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }

        /// <summary>
        /// Finds the amount of principal paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static decimal FindPrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = FindPayment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FindFutureValue(interestRatePerPeriod, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRatePerPeriod;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= (1 + interestRatePerPeriod);
            }

            return payment - interestPayment;
        }

        /// <summary>
        /// Finds the amount of principal paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        public static double FindPrincipalPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            return Convert.ToDouble(FindPrincipalPayment(Convert.ToDecimal(interestRatePerPeriod), periodNumber, numberOfPeriods, Convert.ToDecimal(presentValue), Convert.ToDecimal(futureValue), paymentDueAtBeginningOfPeriod));
        }
    }
}