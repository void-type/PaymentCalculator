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

            assetCostTextBox.Focus();
        }

        private List<SinglePaymentInformation> TableItems = new List<SinglePaymentInformation>();

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

            monthlyPaymentTextBox.Text = string.Format("{0:C2}", calc.MonthlyPayment);
            loanAmountTextBox.Text = string.Format("{0:C2}", calc.LoanAmount);
            interestPaidTextBox.Text = string.Format("{0:C2}", calc.TotalInterestPaid);
            totalPaidTextBox.Text = string.Format("{0:C2}", calc.TotalPaid);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear Input
            assetCostTextBox.Text = "";
            downPaymentTextBox.Text = "";
            interestRateTextBox.Text = "";
            yearsTextBox.Text = "";
            periodsPerYearComboBox.SelectedIndex = 0;

            // Clear Output
            loanAmountTextBox.Text = "";
            interestPaidTextBox.Text = "";
            monthlyPaymentTextBox.Text = "";
            totalPaidTextBox.Text = "";

            // Clear Table
            AmortizationTable.ItemsSource = null;

            // Set caret on the 1st input box
            assetCostTextBox.Focus();
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
            decimal assetCost;
            if ((!decimal.TryParse(assetCostTextBox.Text, out assetCost)) || (assetCost <= 0))
            {
                DisplayMessage("Please enter an asset cost.");
                return null;
            }

            if (string.IsNullOrEmpty(downPaymentTextBox.Text))
            {
                downPaymentTextBox.Text = "0";
            }
            decimal downPayment;
            if ((!decimal.TryParse(downPaymentTextBox.Text, out downPayment)) || (downPayment < 0))
            {
                DisplayMessage("Please enter a down payment greater than or equal to $0.");
                return null;
            }
            if (downPayment >= assetCost)
            {
                DisplayMessage("You have already paid off the loan. The down payment is bigger than the loan.");
                return null;
            }

            decimal interestRate;
            if ((!decimal.TryParse(interestRateTextBox.Text, out interestRate)) || (interestRate <= 0))
            {
                DisplayMessage("Please enter an interest rate.");
                return null;
            }

            int years;
            if ((!int.TryParse(yearsTextBox.Text, out years)) || (years <= 0))
            {
                DisplayMessage("Please enter how many years the payback period is.");
                return null;
            }
            if (years > 450)
            {
                DisplayMessage("Valid year range is 1 to 450 years.");
                return null;
            }

            int periodsPerYear = 12;

            switch (periodsPerYearComboBox.SelectedIndex)
            {
                case 1:
                    periodsPerYear = 4;
                    break;

                case 2:
                    periodsPerYear = 1;
                    break;

                default:
                    periodsPerYearComboBox.SelectedIndex = 0;
                    periodsPerYear = 12;
                    break;
            }

            if ((bool)IsPercentInterestCheckBox.IsChecked)
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