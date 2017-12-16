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

        private void About_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Author: Jeff Schreiner\nThis payment calculator is free to use and distribute.\nSee the source code at GitHub.com/void-type");
        }

        private void calcButton_Click(object sender, RoutedEventArgs e)
        {
            var calc = ScrubInput();

            if (calc == null)
            {
                return;
            }

            try
            {
                AmortizationTable.ItemsSource = calc.MakeTable();
            }
            catch (System.OverflowException)
            {
                MessageBox.Show("The loan is too large to calculate.");
                return;
            }

            MonthlyPaymentTextBox.Text = $"{calc.MonthlyPayment:C2}";
            LoanAmountTextBox.Text = $"{calc.LoanAmount:C2}";
            InterestPaidTextBox.Text = $"{calc.TotalInterestPaid:C2}";
            TotalPaidTextBox.Text = $"{calc.TotalPaid:C2}";
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
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

        private void DisplayMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        private void periodsPerYearComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var options = new List<string>()
            {
                "Monthly", "Quarterly", "Yearly"
            };

            var cmbx = sender as ComboBox;

            cmbx.ItemsSource = options;
            cmbx.SelectedIndex = 0;
        }

        private AmortizationCalculator ScrubInput()
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

            if ((!decimal.TryParse(InterestRateTextBox.Text, out var interestRate)) || (interestRate <= 0))
            {
                DisplayMessage("Please enter an interest rate.");
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

            if (IsPercentInterestCheckBox.IsChecked != null && (bool)IsPercentInterestCheckBox.IsChecked)
            {
                interestRate /= 100;
            }

            if (interestRate > 2)
            {
                DisplayMessage("Please enter an interest rate less than 200%.");
                return null;
            }

            return new AmortizationCalculator(assetCost, downPayment, interestRate, years, periodsPerYear);
        }
    }
}