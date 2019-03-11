using System.ComponentModel;

namespace PaymentCalculator.Wpf.Model.Amortization
{
    /// <summary>
    /// A representation of per-payment loan information.
    /// </summary>
    public class Period : IPeriod
    {
        [DisplayName("Balance Remaining")]
        public decimal BalanceLeft { get; set; }

        [DisplayName("Interest")]
        public decimal InterestPayment { get; set; }

        [DisplayName("#")]
        public int PeriodNumber { get; set; }

        [DisplayName("Principal")]
        public decimal PrincipalPayment { get; set; }
    }
}
