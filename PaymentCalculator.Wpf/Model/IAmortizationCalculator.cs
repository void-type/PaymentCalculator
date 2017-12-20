namespace PaymentCalculator.Wpf.Model
{
    public interface IAmortizationCalculator
    {
        void Calculate(ILoan loan);
    }
}