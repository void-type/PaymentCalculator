namespace PaymentCalculator.Wpf.Model.Financial
{
    /// <summary>
    /// Allows the static Financial library to be injected as a dependency.
    /// </summary>
    public interface IFinancial
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
        decimal FindFutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        /// <summary>
        /// Finds the future value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        double FindFutureValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double presentValue = 0, bool paymentDueAtBeginningOfPeriod = false);

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
        decimal FindInterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

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
        double FindInterestPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        /// <summary>
        /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
        /// <returns></returns>
        decimal FindNetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows);

        /// <summary>
        /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
        /// <returns></returns>
        double FindNetPresentValue(double interestRatePerPeriod, params double[] cashFlows);

        /// <summary>
        /// Finds the payment amount per period for an an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        decimal FindPayment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        /// <summary>
        /// Finds the payment amount per period for an an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="presentValue">The present value of the annuity.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        double FindPayment(double interestRatePerPeriod, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        /// <summary>
        /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        decimal FindPresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

        /// <summary>
        /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
        /// </summary>
        /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divded by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
        /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
        /// <param name="payment">The amount paid against the annuity every period.</param>
        /// <param name="futureValue">The future value of the annuity.</param>
        /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
        /// <returns></returns>
        double FindPresentValue(double interestRatePerPeriod, int numberOfPeriods, double payment, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

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
        decimal FindPrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);

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
        double FindPrincipalPayment(double interestRatePerPeriod, int periodNumber, int numberOfPeriods, double presentValue, double futureValue = 0, bool paymentDueAtBeginningOfPeriod = false);
    }
}