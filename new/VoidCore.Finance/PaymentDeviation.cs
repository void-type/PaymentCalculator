namespace VoidCore.Finance
{
    public struct PaymentDeviation
    {
        public PaymentDeviation(int periodNumber, decimal amount)
        {
            PeriodNumber = periodNumber;
            Amount = amount;
        }

        public int PeriodNumber { get; }
        public decimal Amount { get; }
    }
}
