using VoidCore.Finance;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace PaymentCalculator.Model;

public static class CalculateLoan
{
    public class Handler : EventHandlerSyncAbstract<Request, Response>
    {
        protected override IResult<Response> HandleSync(Request request)
        {
            try
            {
                var totalPrincipal = request.AssetCost - request.DownPayment;
                var numberOfPeriods = request.NumberOfYears * request.PeriodsPerYear;
                var ratePerPeriod = request.AnnualInterestRate == 0 ? 0 : request.AnnualInterestRate / request.PeriodsPerYear;

                var amortizationRequest = new AmortizationRequest(totalPrincipal, numberOfPeriods, ratePerPeriod, request.PaymentModifications);

                var amortizationResponse = AmortizationCalculator.Calculate(amortizationRequest);

                var response = new Response
                {
                    TotalPrincipal = totalPrincipal,
                    PaymentPerPeriod = amortizationResponse.PaymentPerPeriod + request.EscrowPerPeriod,
                    TotalInterestPaid = amortizationResponse.TotalInterestPaid,
                    TotalEscrowPaid = request.EscrowPerPeriod * numberOfPeriods,
                    TotalPaid = request.AssetCost + amortizationResponse.TotalInterestPaid + (request.EscrowPerPeriod * numberOfPeriods),
                    Schedule = amortizationResponse.Schedule,
                    Request = request
                };

                return Ok(response);
            }
            catch (OverflowException)
            {
                return Fail(new LoanOverflowFailure());
            }
            catch (AggregateException ex) when (ex.InnerExceptions.All(e => e.GetType() == typeof(OverflowException)))
            {
                return Fail(new LoanOverflowFailure());
            }
        }
    }

    public record Request
    {
        public required decimal AssetCost { get; init; }
        public required decimal DownPayment { get; init; }
        public required decimal EscrowPerPeriod { get; init; }
        public required int NumberOfYears { get; init; }
        public required int PeriodsPerYear { get; init; }
        public required decimal AnnualInterestRate { get; init; }
        public required List<AmortizationPaymentModification> PaymentModifications { get; init; }
    }

    public record Response
    {
        public required decimal TotalPrincipal { get; init; }
        public required decimal PaymentPerPeriod { get; init; }
        public required decimal TotalInterestPaid { get; init; }
        public required decimal TotalEscrowPaid { get; init; }
        public required decimal TotalPaid { get; init; }
        public required IReadOnlyList<AmortizationPeriod> Schedule { get; init; }
        public required Request Request { get; init; }
    }

    public class RequestValidator : RuleValidatorAbstract<Request>
    {
        public RequestValidator()
        {
            CreateRule(r => new Failure("Asset Cost must be positive.", nameof(r.AssetCost)))
                .InvalidWhen(r => r.AssetCost < 0);

            CreateRule(r => new Failure("Down Payment must be less than or equal to the Asset Cost.", nameof(r.DownPayment)))
                .InvalidWhen(r => r.DownPayment > r.AssetCost);

            CreateRule(r => new Failure("Escrow per Period must be positive.", nameof(r.EscrowPerPeriod)))
                .InvalidWhen(r => r.EscrowPerPeriod < 0);

            CreateRule(r => new Failure("Number of Years must be greater than zero.", nameof(r.NumberOfYears)))
                .InvalidWhen(r => r.NumberOfYears < 1);

            CreateRule(r => new Failure("Periods per Year must be greater than zero.", nameof(r.PeriodsPerYear)))
                .InvalidWhen(r => r.PeriodsPerYear < 1);
        }
    }
}
