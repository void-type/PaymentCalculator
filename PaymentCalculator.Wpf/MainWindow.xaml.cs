using PaymentCalculator.Wpf.Model.Amortization;
using PaymentCalculator.Wpf.Model.Financial;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PaymentCalculator.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, int> _periodsPerYearChoices = new Dictionary<string, int>
        { { "Monthly", 12 },
            { "Quarterly", 4 },
            { "Yearly", 1 }
        };

        public MainWindow()
        {
            InitializeComponent();
            AssetCostTextBox.Focus();
        }

        private static void DisplayMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        private void About_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisplayMessage("Author: Jeff Schreiner\nThis payment calculator is free to use and distribute.\nSee the source code at GitHub.com/void-type");
        }

        private async Task CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var loan = ValidateAndSetInputs();

            if (loan == null)
            {
                return;
            }

            try
            {
                var calculator = new AmortizationCalculator(new FinancialWrapper());
                await Task.FromResult(() => calculator.Calculate(loan));
            }
            catch (System.OverflowException)
            {
                DisplayMessage("The loan is too large to calculate.");
                return;
            }

            AmortizationTable.ItemsSource = loan.Schedule;
            MonthlyPaymentTextBox.Text = $"{loan.MonthlyPayment:C2}";
            LoanAmountTextBox.Text = $"{loan.LoanAmount:C2}";
            InterestPaidTextBox.Text = $"{loan.TotalInterestPaid:C2}";
            TotalPaidTextBox.Text = $"{loan.TotalPaid:C2}";
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            AssetCostTextBox.Text = "";
            DownPaymentTextBox.Text = "";
            InterestRateTextBox.Text = "";
            YearsTextBox.Text = "";
            PeriodsPerYearComboBox.SelectedIndex = 0;
            LoanAmountTextBox.Text = "";
            InterestPaidTextBox.Text = "";
            MonthlyPaymentTextBox.Text = "";
            TotalPaidTextBox.Text = "";
            AmortizationTable.ItemsSource = null;
            AssetCostTextBox.Focus();
        }

        private void PeriodsPerYearComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is ComboBox cmbx))
            {
                return;
            }

            cmbx.ItemsSource = _periodsPerYearChoices.Keys;
            cmbx.SelectedIndex = 0;
        }

        private ILoan ValidateAndSetInputs()
        {
            if ((!decimal.TryParse(AssetCostTextBox.Text, out var assetCost)) || (assetCost <= 0))
            {
                DisplayMessage("Please enter an asset cost.");
                return null;
            }

            if (string.IsNullOrEmpty(DownPaymentTextBox.Text))
            {
                DownPaymentTextBox.Text = "0";
            }
            if ((!decimal.TryParse(DownPaymentTextBox.Text, out var downPayment)) || (downPayment < 0))
            {
                DisplayMessage("Please enter a down payment greater than or equal to $0.");
                return null;
            }
            if (downPayment >= assetCost)
            {
                DisplayMessage("You have already paid off the loan. The down payment is bigger than the loan.");
                return null;
            }

            if ((!int.TryParse(YearsTextBox.Text, out var years)) || (years <= 0))
            {
                DisplayMessage("Please enter how many years the payback period is.");
                return null;
            }
            if (years > 450)
            {
                DisplayMessage("Valid year range is 1 to 450 years.");
                return null;
            }

            int periodsPerYear;

            try
            {
                periodsPerYear = _periodsPerYearChoices[PeriodsPerYearComboBox.Text];
            }
            catch (KeyNotFoundException)
            {
                periodsPerYear = 12;
            }

            if ((!decimal.TryParse(InterestRateTextBox.Text, out var interestRate)) || (interestRate <= 0))
            {
                DisplayMessage("Please enter an interest rate.");
                return null;
            }

            if (IsPercentInterestCheckBox.IsChecked != null && (bool) IsPercentInterestCheckBox.IsChecked)
            {
                interestRate /= 100;
            }

            if (interestRate > 2)
            {
                DisplayMessage("Please enter an interest rate less than 200%.");
                return null;
            }

            return new Loan(assetCost, downPayment, years, periodsPerYear, interestRate);
        }
    }
}
