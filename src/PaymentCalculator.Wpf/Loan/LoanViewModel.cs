using VoidCore.Finance;

namespace PaymentCalculator.Wpf
{
    public class LoanViewModel : ViewModelAbstract
    {
        private decimal _assetCost;
        private decimal _annualInterestRate;
        private decimal _downPayment;
        private decimal _escrowPerPeriod;
        private PeriodType _selectedPeriodType = PeriodType.Monthly;
        private int _years = 30;
        private IEnumerable<AmortizationPeriod> _schedule = Array.Empty<AmortizationPeriod>();
        private decimal _paymentPerPeriod;
        private decimal _totalPaid;
        private decimal _totalInterestPaid;
        private decimal _totalEscrowPaid;
        private decimal _totalPrincipal;

        public decimal AssetCost
        {
            get => _assetCost;
            set => SetProperty(ref _assetCost, value);
        }

        public decimal AnnualInterestRate
        {
            get => _annualInterestRate;
            set => SetProperty(ref _annualInterestRate, value);
        }

        public decimal DownPayment
        {
            get => _downPayment;
            set => SetProperty(ref _downPayment, value);
        }

        public decimal EscrowPerPeriod
        {
            get => _escrowPerPeriod;
            set => SetProperty(ref _escrowPerPeriod, value);
        }

        public PeriodType SelectedPeriodType
        {
            get => _selectedPeriodType;
            set => SetProperty(ref _selectedPeriodType, value);
        }

        public static IEnumerable<PeriodType> PeriodTypes => Enum.GetValues(typeof(PeriodType)).Cast<PeriodType>();

        public IEnumerable<AmortizationPeriod> Schedule
        {
            get => _schedule;
            set => SetProperty(ref _schedule, value);
        }

        public decimal PaymentPerPeriod
        {
            get => _paymentPerPeriod;
            set => SetProperty(ref _paymentPerPeriod, value);
        }

        public decimal TotalPrincipal
        {
            get => _totalPrincipal;
            set => SetProperty(ref _totalPrincipal, value);
        }

        public decimal TotalInterestPaid
        {
            get => _totalInterestPaid;
            set => SetProperty(ref _totalInterestPaid, value);
        }

        public decimal TotalEscrowPaid
        {
            get => _totalEscrowPaid;
            set => SetProperty(ref _totalEscrowPaid, value);
        }

        public decimal TotalPaid
        {
            get => _totalPaid;
            set => SetProperty(ref _totalPaid, value);
        }

        public int Years
        {
            get => _years;
            set => SetProperty(ref _years, value);
        }
    }
}
