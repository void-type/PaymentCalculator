using VoidCore.Domain;

namespace PaymentCalculator.Model
{
    public class LoanOverflowFailure : Failure
    {
        public LoanOverflowFailure(string uiHandle = null) : base("The loan is too large to calculate.", uiHandle) { }
    }
}
