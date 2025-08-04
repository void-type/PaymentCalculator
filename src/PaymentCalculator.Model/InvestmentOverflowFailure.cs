using VoidCore.Model.Functional;

namespace PaymentCalculator.Model;

public class InvestmentOverflowFailure : Failure
{
    public InvestmentOverflowFailure(string? uiHandle = null) : base("The investment is too large to calculate.", uiHandle) { }
}
