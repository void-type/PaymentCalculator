namespace PaymentCalculator.Model.Amortization
{
    public class Period
    {
        public Period(int periodNumber, decimal interestPayment, decimal principalPayment, decimal balanceLeft)
        {
            this.PeriodNumber = periodNumber;
            this.InterestPayment = interestPayment;
            this.PrincipalPayment = principalPayment;
            this.BalanceLeft = balanceLeft;
        }

        public int PeriodNumber { get; }
        public decimal InterestPayment { get; }
        public decimal PrincipalPayment { get; }
        public decimal BalanceLeft { get; }
    }
}
