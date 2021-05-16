using PaymentCalculator.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VoidCore.Finance;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace PaymentCalculator.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEventHandler<CalculateLoan.Request, CalculateLoan.Response> _calculateLoanHandler;

        public MainWindow()
        {
            InitializeComponent();

            SelectAllTextBoxBehavior.AddBehavior(AssetCostTextBox, DownPaymentTextBox, AnnualInterestRateTextBox, YearsTextBox, EscrowPerPeriodTextBox);

            _calculateLoanHandler = new CalculateLoan.Handler(new AmortizationCalculator(new Financial()))
                .AddRequestValidator(new CalculateLoan.RequestValidator());

            Clear();
        }

        private void About_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var version = ThisAssembly.AssemblyInformationalVersion;
            MessageBox.Show($"Version: {version}\n\nAuthor: Jeff Schreiner\n\nThis payment calculator is free to use and distribute.\n\nSee the source code at https://github.com/void-type/PaymentCalculator", "About Payment Calculator");
        }

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            Calc();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private CalculateLoan.Request GetRequestFromViewModel()
        {
            var viewModel = (LoanViewModel)DataContext;

            return new CalculateLoan.Request(
                assetCost: viewModel.AssetCost,
                downPayment: viewModel.DownPayment,
                escrowPerPeriod: viewModel.EscrowPerPeriod,
                numberOfYears: viewModel.Years,
                periodsPerYear: (int)viewModel.SelectedPeriodType,
                annualInterestRate: viewModel.AnnualInterestRate / 100
            );
        }

        private static void ShowFailureMessages(IEnumerable<IFailure> failures)
        {
            MessageBox.Show(string.Join("\n", failures.Select(f => f.Message)));
        }

        private void ShowCalculationResponse(CalculateLoan.Response response)
        {
            var viewModel = (LoanViewModel)DataContext;

            viewModel.TotalPrincipal = response.TotalPrincipal;
            viewModel.PaymentPerPeriod = response.PaymentPerPeriod;
            viewModel.TotalInterestPaid = response.TotalInterestPaid;
            viewModel.TotalEscrowPaid = response.TotalEscrowPaid;
            viewModel.TotalPaid = response.TotalPaid;
            viewModel.Schedule = response.Schedule;
        }

        // TODO: WPF doesn't support async interations.
#pragma warning disable CS4014
        private void Calc()
        {
            var request = GetRequestFromViewModel();

            _calculateLoanHandler
                .Handle(request)
                .TeeOnFailureAsync(failures => ShowFailureMessages(failures))
                .TeeOnSuccessAsync(response => ShowCalculationResponse(response));
        }
#pragma warning restore CS4014

        private void Clear()
        {
            DataContext = new LoanViewModel();
            AssetCostTextBox.Focus();
        }
    }
}
