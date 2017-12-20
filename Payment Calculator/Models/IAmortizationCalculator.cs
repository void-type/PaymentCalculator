namespace PaymentCalculator.Models
{
    public interface IAmortizationCalculator
    {
        void Calculate(ILoan loan);
    }
}