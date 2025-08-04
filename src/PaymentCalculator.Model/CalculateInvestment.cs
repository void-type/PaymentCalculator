using VoidCore.Finance;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace PaymentCalculator.Model;

public static class CalculateInvestment
{
    public class Handler : EventHandlerSyncAbstract<InvestmentRequest, InvestmentResponse>
    {
        protected override IResult<InvestmentResponse> HandleSync(InvestmentRequest request)
        {
            try
            {
                var response = InvestmentCalculator.Calculate(request);

                return Ok(response);
            }
            catch (OverflowException)
            {
                return Fail(new InvestmentOverflowFailure());
            }
            catch (AggregateException ex) when (ex.InnerExceptions.All(e => e.GetType() == typeof(OverflowException)))
            {
                return Fail(new InvestmentOverflowFailure());
            }
        }
    }

    public class RequestValidator : RuleValidatorAbstract<InvestmentRequest>
    {
        public RequestValidator()
        {
            CreateRule(r => new Failure("Initial Investment must be positive.", nameof(r.InitialInvestment)))
                .InvalidWhen(r => r.InitialInvestment < 0);

            CreateRule(r => new Failure("Periodic Contribution must be positive.", nameof(r.PeriodicContribution)))
                .InvalidWhen(r => r.PeriodicContribution < 0);

            CreateRule(r => new Failure("Number of Periods must be greater than zero.", nameof(r.NumberOfPeriods)))
                .InvalidWhen(r => r.NumberOfPeriods < 1);
        }
    }
}
