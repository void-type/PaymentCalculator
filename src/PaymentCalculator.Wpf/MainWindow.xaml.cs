using PaymentCalculator.Model;
using System.Linq;
using System.Windows;
using VoidCore.Finance;

namespace PaymentCalculator.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly AmortizationCalculator _amortizationCalculator = new AmortizationCalculator(new Financial());

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new LoanViewModel();
            AssetCostTextBox.Focus();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new LoanViewModel();
            AssetCostTextBox.Focus();
        }

        private void About_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Author: Jeff Schreiner\nThis payment calculator is free to use and distribute.\nSee the source code at https://github.com/void-type");
        }

        private async void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoanViewModel) DataContext;

            var interestRate = viewModel.IsPercentInterest ?
                viewModel.AnnualInterestRate / 100 :
                viewModel.AnnualInterestRate;

            var request = new CalculateLoan.Request(
                assetCost: viewModel.AssetCost,
                downPayment: viewModel.DownPayment, escrowPerPeriod: 0,
                numberOfYears: viewModel.Years,
                periodsPerYear: (int) viewModel.SelectedPeriodType,
                annualInterestRate : interestRate
            );

            var result = await new CalculateLoan.Handler(_amortizationCalculator)
                .AddRequestValidator(new CalculateLoan.RequestValidator())
                .Handle(request);

            if (result.IsFailed)
            {
                MessageBox.Show(string.Join("\n", result.Failures.Select(f => f.Message)));
                return;
            }

            var response = result.Value;

            viewModel.Schedule = response.Schedule;
            viewModel.PaymentPerPeriod = response.PaymentPerPeriod;
            viewModel.TotalPrincipal = response.TotalPrincipal;
            viewModel.TotalInterestPaid = response.TotalInterestPaid;
            viewModel.TotalPaid = response.TotalPaid;
        }
    }
}
