using System;

namespace VoidCore.Finance
{
    /// <summary>
    /// This class has the functionality of the VB.Net and Excel Financial functions, which specialize in time-value of money.
    /// </summary>
    public class Financial : IFinancial
    {
        /// <summary>
        /// Finds the future value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        public decimal FutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            if (paymentDueAtBeginningOfPeriod)
            {
                payment = payment * (1 + interestRatePerPeriod);
            }

            if (numberOfPeriods == 0)
            {
                return -presentValue;
            }
            else if (interestRatePerPeriod == 0)
            {
                return -(presentValue + (numberOfPeriods * payment));
            }
            else
            {
                var pow = (decimal) Math.Pow(1 + (double) interestRatePerPeriod, numberOfPeriods);
                return -1 * (payment * (pow - 1) / interestRatePerPeriod + presentValue * pow);
            }
        }

        /// <summary>
        /// Finds the amount of interest paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        public decimal InterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = Payment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FutureValue(interestRatePerPeriod, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRatePerPeriod;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= (1 + interestRatePerPeriod);
            }

            return interestPayment;
        }

        /// <summary>
        /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
        public decimal NetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows)
        {
            var netPresentValue = 0.0m;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / (decimal) Math.Pow((1 + (double) interestRatePerPeriod), (i + 1));
            }

            return netPresentValue;
        }

        /// <summary>
        /// Finds the payment amount per period for an an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        public decimal Payment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
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
                payment = interestRatePerPeriod / (decimal) (Math.Pow(1 + (double) interestRatePerPeriod, numberOfPeriods) - 1) * -(presentValue * (decimal) Math.Pow(1 + (double) interestRatePerPeriod, numberOfPeriods) + futureValue);
            }

            if (paymentDueAtBeginningOfPeriod)
            {
                payment /= (1 + interestRatePerPeriod);
            }

            return payment;
        }

        /// <summary>
        /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        public decimal PresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var num = 1.0m;
            var pow = (decimal) Math.Pow((1 + (double) interestRatePerPeriod), numberOfPeriods);

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
        /// Finds the amount of principal paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="periodNumber">The period number in which to find the interest paid.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        public decimal PrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = Payment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FutureValue(interestRatePerPeriod, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRatePerPeriod;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= (1 + interestRatePerPeriod);
            }

            return payment - interestPayment;
        }
    }
}
