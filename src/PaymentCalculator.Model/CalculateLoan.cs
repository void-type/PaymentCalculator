using System.Collections.Generic;
using VoidCore.Domain;
using VoidCore.Domain.Events;
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
                var amortizationRequest = new AmortizationRequest(request.TotalPrincipal, request.NumberOfPeriods, request.RatePerPeriod);

                var amortizationResponse = _amortizationCalculator.Calculate(amortizationRequest);

                var response = new Response(
                    paymentPerPeriod: amortizationResponse.PaymentPerPeriod + request.EscrowPerPeriod,
                    totalInterestPaid: amortizationResponse.TotalInterestPaid,
                    totalPaid: request.AssetCost + amortizationResponse.TotalInterestPaid + request.EscrowPerPeriod * request.NumberOfPeriods,
                    schedule: amortizationResponse.Schedule,
                    request: request);

                return Result.Ok(response);
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
                TotalPrincipal = assetCost - downPayment;
                NumberOfPeriods = numberOfYears * periodsPerYear;
                RatePerPeriod = annualInterestRate / periodsPerYear;
            }

            public decimal AssetCost { get; }
            public decimal DownPayment { get; }
            public decimal EscrowPerPeriod { get; }
            public int NumberOfYears { get; }
            public int PeriodsPerYear { get; }
            public decimal AnnualInterestRate { get; }
            public decimal TotalPrincipal { get; }
            public int NumberOfPeriods { get; }
            public decimal RatePerPeriod { get; }
        }

        public class Response
        {
            internal Response(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<AmortizationPeriod> schedule, Request request)
            {
                PaymentPerPeriod = paymentPerPeriod;
                TotalInterestPaid = totalInterestPaid;
                TotalPaid = totalPaid;
                Schedule = schedule;
                Request = request;
            }

            public decimal PaymentPerPeriod { get; }
            public decimal TotalInterestPaid { get; }
            public decimal TotalPaid { get; }
            public IReadOnlyList<AmortizationPeriod> Schedule { get; }
            public Request Request { get; }
        }
    }
}
