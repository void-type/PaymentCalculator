namespace PaymentCalculator.Wpf.Model.Amoritization
{
    public interface IAmortizationCalculator
    {
        void Calculate(ILoan loan);
    }
}