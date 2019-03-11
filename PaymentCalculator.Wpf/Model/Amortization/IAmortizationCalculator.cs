namespace PaymentCalculator.Wpf.Model.Amortization
{
    public interface IAmortizationCalculator
    {
        void Calculate(ILoan loan);
    }
}
