using System.Collections.Generic;

namespace PaymentCalculator.Models.Amoritization
{
    /// <summary>
    /// A view model that represents and amoritized loan.
    /// </summary>
    public class AmoritizationViewModel
    {
        /// <summary>
        /// Amoritization schedule containing a list of periods. Can be used to populate an amoritization table.
        /// </summary>
        public List<Period> Schedule { get; } = new List<Period>();

        /// <summary>
        /// Static information about the loan that's used to build the amoritization schedule. The loan also contains stats that are pulled from the entire life of the loan.
        /// </summary>
        public Loan Loan { get; } = new Loan();
    }
}