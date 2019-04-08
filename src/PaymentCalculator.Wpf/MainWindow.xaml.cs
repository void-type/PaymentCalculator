using PaymentCalculator.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Finance;

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

            _calculateLoanHandler = new CalculateLoan.Handler(new AmortizationCalculator(new Financial()))
                .AddRequestValidator(new CalculateLoan.RequestValidator());

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
            var request = GetRequestFromViewModel();

            await _calculateLoanHandler
                .Handle(request)
                .TeeOnFailureAsync(failures => ShowFailureMessages(failures))
                .TeeOnSuccessAsync(response => ShowResponse(response));
        }

        private CalculateLoan.Request GetRequestFromViewModel()
        {
            var viewModel = (LoanViewModel) DataContext;

            return new CalculateLoan.Request(
                assetCost: viewModel.AssetCost,
                downPayment: viewModel.DownPayment,
                escrowPerPeriod: 0,
                numberOfYears: viewModel.Years,
                periodsPerYear: (int) viewModel.SelectedPeriodType,
                annualInterestRate : viewModel.IsPercentInterest ? viewModel.AnnualInterestRate / 100 : viewModel.AnnualInterestRate);
        }

        private void ShowFailureMessages(IEnumerable<IFailure> failures)
        {
            MessageBox.Show(string.Join("\n", failures.Select(f => f.Message)));
        }

        private void ShowResponse(CalculateLoan.Response response)
        {
            var viewModel = (LoanViewModel) DataContext;

            viewModel.TotalPrincipal = response.TotalPrincipal;
            viewModel.PaymentPerPeriod = response.PaymentPerPeriod;
            viewModel.TotalInterestPaid = response.TotalInterestPaid;
            viewModel.TotalPaid = response.TotalPaid;
            viewModel.Schedule = response.Schedule;
        }
    }
}
