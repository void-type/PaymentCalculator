using PaymentCalculator.Models.Amoritization;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PaymentCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new AmoritizationViewModel();

            if (!ValidateAndSetInputs(viewModel))
            {
                return;
            }

            try
            {
                AmortizationCalculator.Calculate(viewModel);
            }
            catch (System.OverflowException)
            {
                DisplayMessage("The loan is too large to calculate.");
                return;
            }

            AmortizationTable.ItemsSource = viewModel.Schedule;
            MonthlyPaymentTextBox.Text = $"{viewModel.Loan.MonthlyPayment:C2}";
            LoanAmountTextBox.Text = $"{viewModel.Loan.LoanAmount:C2}";
            InterestPaidTextBox.Text = $"{viewModel.Loan.TotalInterestPaid:C2}";
            TotalPaidTextBox.Text = $"{viewModel.Loan.TotalPaid:C2}";
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear Input
            AssetCostTextBox.Text = "";
            DownPaymentTextBox.Text = "";
            InterestRateTextBox.Text = "";
            YearsTextBox.Text = "";
            PeriodsPerYearComboBox.SelectedIndex = 0;

            // Clear Output
            LoanAmountTextBox.Text = "";
            InterestPaidTextBox.Text = "";
            MonthlyPaymentTextBox.Text = "";
            TotalPaidTextBox.Text = "";

            // Clear Table
            AmortizationTable.ItemsSource = null;

            // Set caret on the 1st input box
            AssetCostTextBox.Focus();
        }

        private void PeriodsPerYearComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var options = new List<string>()
            {
                "Monthly", "Quarterly", "Yearly"
            };

            if (!(sender is ComboBox cmbx))
            {
                return;
            }

            cmbx.ItemsSource = options;
            cmbx.SelectedIndex = 0;
        }

        private bool ValidateAndSetInputs(AmoritizationViewModel viewModel)
        {
            var loan = viewModel.Loan;

            if ((!decimal.TryParse(AssetCostTextBox.Text, out var assetCost)) || (assetCost <= 0))
            {
                DisplayMessage("Please enter an asset cost.");
                return false;
            }

            if (string.IsNullOrEmpty(DownPaymentTextBox.Text))
            {
                DownPaymentTextBox.Text = "0";
            }
            if ((!decimal.TryParse(DownPaymentTextBox.Text, out var downPayment)) || (downPayment < 0))
            {
                DisplayMessage("Please enter a down payment greater than or equal to $0.");
                return false;
            }
            if (downPayment >= assetCost)
            {
                DisplayMessage("You have already paid off the loan. The down payment is bigger than the loan.");
                return false;
            }

            if ((!int.TryParse(YearsTextBox.Text, out var years)) || (years <= 0))
            {
                DisplayMessage("Please enter how many years the payback period is.");
                return false;
            }
            if (years > 450)
            {
                DisplayMessage("Valid year range is 1 to 450 years.");
                return false;
            }

            int periodsPerYear;

            switch (PeriodsPerYearComboBox.SelectedIndex)
            {
                case 1:
                    periodsPerYear = 4;
                    break;

                case 2:
                    periodsPerYear = 1;
                    break;

                default:
                    PeriodsPerYearComboBox.SelectedIndex = 0;
                    periodsPerYear = 12;
                    break;
            }

            if ((!decimal.TryParse(InterestRateTextBox.Text, out var interestRate)) || (interestRate <= 0))
            {
                DisplayMessage("Please enter an interest rate.");
                return false;
            }

            if (IsPercentInterestCheckBox.IsChecked != null && (bool)IsPercentInterestCheckBox.IsChecked)
            {
                interestRate /= 100;
            }

            if (interestRate > 2)
            {
                DisplayMessage("Please enter an interest rate less than 200%.");
                return false;
            }

            loan.AssetCost = assetCost;
            loan.DownPayment = downPayment;
            loan.Years = years;
            loan.PeriodsPerYear = periodsPerYear;
            loan.InterestRate = interestRate;

            return true;
        }
    }
}