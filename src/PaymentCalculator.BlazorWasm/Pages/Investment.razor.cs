using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using PaymentCalculator.Model;
using System.ComponentModel.DataAnnotations;
using VoidCore.Finance;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace PaymentCalculator.BlazorWasm.Pages;

public partial class Investment : ComponentBase
{
    private static readonly Dictionary<string, int> _contributionFrequencies = new()
    {
        ["Monthly"] = 12,
        ["Quarterly"] = 4,
        ["Yearly"] = 1
    };

    private static readonly EventHandlerDecorator<InvestmentRequest, InvestmentResponse> _calculateInvestmentHandler =
        new CalculateInvestment.Handler()
            .AddRequestValidator(new CalculateInvestment.RequestValidator());

    private EditContext? _editContext;
    private InvestmentInputViewModel _inputModel = new();
    private InvestmentOutputViewModel _outputModel = new();
    private IEnumerable<string> _validationMessages = [];
    private IEnumerable<string> _validationFields = [];

    protected override void OnInitialized()
    {
        _editContext = new EditContext(_inputModel);
        Clear();
    }

    private async Task CalcAsync()
    {
        ShowFailureMessages([]);

        var request = ConvertToRequest(_inputModel);

        await _calculateInvestmentHandler
            .Handle(request)
            .TeeOnFailureAsync(failures => ShowFailureMessages(failures))
            .TeeOnSuccessAsync(async response => await ShowOutput(response));
    }

    private static InvestmentRequest ConvertToRequest(InvestmentInputViewModel inputModel)
    {
        var numberOfPeriods = inputModel.NumberOfYears * inputModel.ContributionFrequency;
        var ratePerPeriod = (inputModel.AnnualReturnRate / 100) / inputModel.ContributionFrequency;

        return new InvestmentRequest(
            inputModel.InitialInvestment,
            inputModel.PeriodicContribution,
            numberOfPeriods,
            ratePerPeriod);
    }

    private async Task ShowOutput(InvestmentResponse response)
    {
        var totalGain = response.FinalValue - response.Request.InitialInvestment - response.TotalContributions;

        _outputModel = new InvestmentOutputViewModel
        {
            FinalValue = $"{response.FinalValue:c}",
            TotalContributions = $"{response.TotalContributions:c}",
            TotalInterestEarned = $"{response.TotalInterestEarned:c}",
            TotalGain = $"{totalGain:c}",
            Schedule = response.Schedule
                .Select(p => new InvestmentOutputPeriodViewModel(
                    p.PeriodNumber.ToString(),
                    $"{p.Contribution:c}",
                    $"{p.InterestEarned:c}",
                    $"{p.PeriodEndBalance:c}"))
                .ToList()
        };

        await Task.CompletedTask;
    }

    private void ShowFailureMessages(IEnumerable<IFailure> failures)
    {
        _validationMessages = failures.Select(f => f.Message);
        _validationFields = failures
            .Select(f => f.UiHandle)
            .Where(f => f is not null)
            .Select(f => f!);
    }

    private void Clear()
    {
        _inputModel = new InvestmentInputViewModel();
        _outputModel = new InvestmentOutputViewModel();
        _validationMessages = [];
        _validationFields = [];

        _editContext = new EditContext(_inputModel);
    }

    private async Task HandleKeyPressAsync(KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Key == "Enter")
        {
            await CalcAsync();
        }
    }

    private string GetInputCssForField(string fieldName)
    {
        return string.Join(" ",
            "form-control",
            (_validationFields.Contains(fieldName) ? "invalid" : null));
    }

    private sealed class InvestmentInputViewModel
    {
        [Display(Name = "Initial Investment")]
        public decimal InitialInvestment { get; set; }

        [Display(Name = "Periodic Contribution")]
        public decimal PeriodicContribution { get; set; }

        [Display(Name = "Annual Return Rate")]
        public decimal AnnualReturnRate { get; set; } = 7;

        [Display(Name = "Number of Years")]
        public int NumberOfYears { get; set; } = 30;

        [Display(Name = "Contribution Frequency")]
        public int ContributionFrequency { get; set; } = 12;
    }

    private sealed class InvestmentOutputViewModel
    {
        public string FinalValue { get; set; } = string.Empty;
        public string TotalContributions { get; set; } = string.Empty;
        public string TotalInterestEarned { get; set; } = string.Empty;
        public string TotalGain { get; set; } = string.Empty;
        public IReadOnlyList<InvestmentOutputPeriodViewModel> Schedule { get; set; } = new List<InvestmentOutputPeriodViewModel>();
    }

    private sealed class InvestmentOutputPeriodViewModel
    {
        public InvestmentOutputPeriodViewModel(string periodNumber, string contribution, string interestEarned, string periodEndBalance)
        {
            PeriodNumber = periodNumber;
            Contribution = contribution;
            InterestEarned = interestEarned;
            PeriodEndBalance = periodEndBalance;
        }

        public string PeriodNumber { get; }
        public string Contribution { get; }
        public string InterestEarned { get; }
        public string PeriodEndBalance { get; }
    }
}
