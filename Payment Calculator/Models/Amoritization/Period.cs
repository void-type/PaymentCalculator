using System.ComponentModel;

namespace PaymentCalculator.Models.Amoritization
{
    /// <summary>
    /// A representation of per-payment loan information.
    /// </summary>
    public class Period
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