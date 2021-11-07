using System.Collections.Generic;
using System.Linq;
using VoidCore.Finance;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace PaymentCalculator.Model
{
    public static class CalculateLoan
    {
        public class Handler : EventHandlerSyncAbstract<Request, Response>
        {
            private readonly AmortizationCalculator _amortizationCalculator;

            public Handler(AmortizationCalculator amortizationCalculator)
            {
                _amortizationCalculator = amortizationCalculator;
            }

            protected override IResult<Response> HandleSync(Request request)
            {
                try
                {
                    var totalPrincipal = request.AssetCost - request.DownPayment;
                    var numberOfPeriods = request.NumberOfYears * request.PeriodsPerYear;
                    var ratePerPeriod = request.AnnualInterestRate == 0 ? 0 : request.AnnualInterestRate / request.PeriodsPerYear;

                    var amortizationRequest = new AmortizationRequest(totalPrincipal, numberOfPeriods, ratePerPeriod);

                    var amortizationResponse = _amortizationCalculator.Calculate(amortizationRequest);

                    var response = new Response(
                        totalPrincipal: totalPrincipal,
                        paymentPerPeriod: amortizationResponse.PaymentPerPeriod + request.EscrowPerPeriod,
                        totalInterestPaid: amortizationResponse.TotalInterestPaid,
                        totalEscrowPaid: request.EscrowPerPeriod * numberOfPeriods,
                        totalPaid: request.AssetCost + amortizationResponse.TotalInterestPaid + (request.EscrowPerPeriod * numberOfPeriods),
                        schedule: amortizationResponse.Schedule);

                    return Ok(response);
                }
                catch (System.AggregateException ex) when (ex.InnerExceptions.All(e => e.GetType() == typeof(System.OverflowException)))
                {
                    return Fail(new LoanOverflowFailure());
                }
            }
        }

        public class Request
        {
            public Request(decimal assetCost, decimal downPayment, decimal escrowPerPeriod, int numberOfYears, int periodsPerYear, decimal annualInterestRate)
            {
                AssetCost = assetCost;
                DownPayment = downPayment;
                EscrowPerPeriod = escrowPerPeriod;
                NumberOfYears = numberOfYears;
                PeriodsPerYear = periodsPerYear;
                AnnualInterestRate = annualInterestRate;
            }

            public decimal AssetCost { get; }
            public decimal DownPayment { get; }
            public decimal EscrowPerPeriod { get; }
            public int NumberOfYears { get; }
            public int PeriodsPerYear { get; }
            public decimal AnnualInterestRate { get; }
        }

        public class Response
        {
            internal Response(decimal totalPrincipal, decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalEscrowPaid, decimal totalPaid, IReadOnlyList<AmortizationPeriod> schedule)
            {
                TotalPrincipal = totalPrincipal;
                PaymentPerPeriod = paymentPerPeriod;
                TotalInterestPaid = totalInterestPaid;
                TotalEscrowPaid = totalEscrowPaid;
                TotalPaid = totalPaid;
                Schedule = schedule;
            }

            public decimal TotalPrincipal { get; }
            public decimal PaymentPerPeriod { get; }
            public decimal TotalInterestPaid { get; }
            public decimal TotalEscrowPaid { get; }
            public decimal TotalPaid { get; }
            public IReadOnlyList<AmortizationPeriod> Schedule { get; }
        }

        public class RequestValidator : RuleValidatorAbstract<Request>
        {
            public RequestValidator()
            {
                CreateRule(r => new Failure("The down payment must be less than the asset cost.", nameof(r.DownPayment)))
                    .InvalidWhen(r => r.DownPayment >= r.AssetCost)
                    .ExceptWhen(r => r.AssetCost == 0);

                CreateRule(r => new Failure("The term must be greater than zero.", nameof(r.NumberOfYears)))
                    .InvalidWhen(r => r.NumberOfYears <= 0);

                CreateRule(r => new Failure("Periods per year must be greater than zero.", nameof(r.PeriodsPerYear)))
                    .InvalidWhen(r => r.PeriodsPerYear <= 0);
            }
        }
    }
}
