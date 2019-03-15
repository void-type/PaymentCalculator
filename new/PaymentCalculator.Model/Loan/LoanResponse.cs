using PaymentCalculator.Model.Amortization;
using System.Collections.Generic;

namespace PaymentCalculator.Model.Loan
{
  public class LoanResponse
  {
    public LoanResponse(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<Period> schedule, LoanRequest request)
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
    public IReadOnlyList<Period> Schedule { get; }
    public LoanRequest Request { get; }
  }
}
