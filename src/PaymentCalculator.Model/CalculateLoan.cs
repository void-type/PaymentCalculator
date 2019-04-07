using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Domain.RuleValidator;
using VoidCore.Finance;

namespace PaymentCalculator.Model
{
    public class CalculateLoan
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
                    var ratePerPeriod = request.AnnualInterestRate / request.PeriodsPerYear;

                    var amortizationRequest = new AmortizationRequest(totalPrincipal, numberOfPeriods, ratePerPeriod);

                    var amortizationResponse = _amortizationCalculator.Calculate(amortizationRequest);

                    var response = new Response(
                        totalPrincipal: totalPrincipal,
                        paymentPerPeriod: amortizationResponse.PaymentPerPeriod + request.EscrowPerPeriod,
                        totalInterestPaid: amortizationResponse.TotalInterestPaid,
                        totalPaid: request.AssetCost + amortizationResponse.TotalInterestPaid + request.EscrowPerPeriod * numberOfPeriods,
                        schedule: amortizationResponse.Schedule,
                        request: request);

                    return Result.Ok(response);
                }
                catch (System.AggregateException ex)
                {
                    if (ex.InnerExceptions.All(e => e.GetType() == typeof(System.OverflowException)))
                    {
                        return Result.Fail<Response>(new LoanOverflowFailure());
                    }
                    else
                    {
                        throw;
                    }
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
            internal Response(decimal totalPrincipal, decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<AmortizationPeriod> schedule, Request request)
            {
                TotalPrincipal = totalPrincipal;
                PaymentPerPeriod = paymentPerPeriod;
                TotalInterestPaid = totalInterestPaid;
                TotalPaid = totalPaid;
                Schedule = schedule;
                Request = request;
            }

            public decimal TotalPrincipal { get; set; }
            public decimal PaymentPerPeriod { get; }
            public decimal TotalInterestPaid { get; }
            public decimal TotalPaid { get; }
            public IReadOnlyList<AmortizationPeriod> Schedule { get; }
            public Request Request { get; }
        }

        public class RequestValidator : RuleValidatorAbstract<Request>
        {
            public RequestValidator()
            {
                CreateRule(r => new Failure("The down payment must be less than the asset cost.", nameof(r.DownPayment)))
                    .InvalidWhen(r => r.DownPayment >= r.AssetCost);

                CreateRule(r => new Failure("The term must be greater than zero.", nameof(r.NumberOfYears)))
                    .InvalidWhen(r => r.NumberOfYears <= 0);

                CreateRule(r => new Failure("Periods per year must be greater than zero.", nameof(r.PeriodsPerYear)))
                    .InvalidWhen(r => r.PeriodsPerYear <= 0);
            }
        }
    }
}
