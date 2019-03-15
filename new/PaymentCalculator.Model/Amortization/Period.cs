namespace PaymentCalculator.Model.Amortization
{
    public class Period
    {
        public Period(int periodNumber, decimal interestPayment, decimal principalPayment, decimal balanceLeft)
        {
            PeriodNumber = periodNumber;
            InterestPayment = interestPayment;
            PrincipalPayment = principalPayment;
            BalanceLeft = balanceLeft;
        }

        public int PeriodNumber { get; }
        public decimal InterestPayment { get; }
        public decimal PrincipalPayment { get; }
        public decimal BalanceLeft { get; }
    }
}
