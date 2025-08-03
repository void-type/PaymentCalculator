using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using PaymentCalculator.Model;
using System.ComponentModel.DataAnnotations;
using VoidCore.Finance;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace PaymentCalculator.BlazorWasm.Pages;

public partial class Index : ComponentBase
{
    private static readonly Dictionary<string, int> _periodFrequencies = new()
    {
        ["Monthly"] = 12,
        ["Quarterly"] = 4,
        ["Yearly"] = 1
    };

    private static readonly EventHandlerDecorator<CalculateLoan.Request, CalculateLoan.Response> _calculateLoanHandler =
        new CalculateLoan.Handler()
            .AddRequestValidator(new CalculateLoan.RequestValidator());

    private EditContext? _editContext;
    private AmortizationInputViewModel _inputModel = new();
    private AmortizationOutputViewModel _outputModel = new();
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

        await _calculateLoanHandler
            .Handle(request)
            .TeeOnFailureAsync(failures => ShowFailureMessages(failures))
            .TeeOnSuccessAsync(async response => await ShowOutput(response));
    }

    private static CalculateLoan.Request ConvertToRequest(AmortizationInputViewModel inputModel, bool ignoreModifications = false)
    {
        var modifications = inputModel.PaymentModifications
            .Select(m => new AmortizationPaymentModification(m.PeriodNumber, m.ModificationAmount))
            .ToList();

        return new CalculateLoan.Request
        {
            AssetCost = inputModel.AssetCost,
            DownPayment = inputModel.DownPayment,
            EscrowPerPeriod = inputModel.EscrowPerPeriod,
            NumberOfYears = inputModel.NumberOfYears,
            PeriodsPerYear = inputModel.PeriodsPerYear,
            AnnualInterestRate = inputModel.AnnualInterestRate / 100,
            PaymentModifications = ignoreModifications ? [] : modifications
        };
    }

    private async Task ShowOutput(CalculateLoan.Response response)
    {
        var interestSaved = 0m;
        var periodsSaved = 0;

        if (response.Request.PaymentModifications.Any())
        {
            var request = response.Request with
            {
                PaymentModifications = []
            };

            var standardResponseResult = await _calculateLoanHandler.Handle(request);

            if (standardResponseResult.IsSuccess)
            {
                var standardResponse = standardResponseResult.Value;
                interestSaved = standardResponse.TotalInterestPaid - response.TotalInterestPaid;
                periodsSaved = response.Schedule.Count(p => p.PrincipalPayment <= 0);
            }
        }

        _outputModel = new AmortizationOutputViewModel
        {
            TotalPrincipal = $"{response.TotalPrincipal:c}",
            PaymentPerPeriod = $"{response.PaymentPerPeriod:c}",
            TotalInterestPaid = $"{response.TotalInterestPaid:c}",
            TotalEscrowPaid = $"{response.TotalEscrowPaid:c}",
            TotalPaid = $"{response.TotalPaid:c}",
            InterestSaved = interestSaved > 0 ? $"{interestSaved:c}" : string.Empty,
            PeriodsSaved = periodsSaved > 0 ? $"{periodsSaved}" : string.Empty,
            Schedule = response.Schedule
                .Select(p => new AmortizationOutputPeriodViewModel(
                    p.PeriodNumber,
                    p.InterestPayment,
                    p.PrincipalPayment,
                    p.BalanceLeft))
                .ToList()
        };
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
        _inputModel = new AmortizationInputViewModel();
        _outputModel = new AmortizationOutputViewModel();
        _validationMessages = [];
        _validationFields = [];

        _editContext = new EditContext(_inputModel);
    }

    private async Task HandleKeyPressAsync(KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Key == "enter")
        {
            await CalcAsync();
        }
    }

    private decimal GetModificationForPeriod(string periodNumberStr)
    {
        if (int.TryParse(periodNumberStr, out int periodNumber))
        {
            return _inputModel.PaymentModifications
                .FirstOrDefault(x => x.PeriodNumber == periodNumber)?.ModificationAmount ?? 0m;
        }
        return 0m;
    }

    private async Task SetModificationForPeriod(string periodNumberStr, decimal amount)
    {
        if (!_outputModel.Schedule.Any())
        {
            return;
        }

        if (int.TryParse(periodNumberStr, out int periodNumber))
        {
            var existing = _inputModel.PaymentModifications.FirstOrDefault(x => x.PeriodNumber == periodNumber);
            if (existing != null)
            {
                if (amount > 0)
                {
                    existing.ModificationAmount = amount;
                }
                else
                {
                    _inputModel.PaymentModifications.Remove(existing);
                }
            }
            else if (amount > 0)
            {
                _inputModel.PaymentModifications.Add(new AmortizationInputPaymentModificationViewModel
                {
                    PeriodNumber = periodNumber,
                    ModificationAmount = amount
                });
            }
        }

        await CalcAsync();
    }

    private string GetInputCssForField(string fieldName)
    {
        return string.Join(" ",
            "form-control",
            (_validationFields.Contains(fieldName) ? "invalid" : null));
    }

    private sealed class AmortizationInputViewModel
    {
        [Display(Name = "Asset Cost")]
        public decimal AssetCost { get; set; }

        [Display(Name = "Down Payment")]
        public decimal DownPayment { get; set; }

        [Display(Name = "Escrow per Period")]
        public decimal EscrowPerPeriod { get; set; }

        [Display(Name = "Number of Years")]
        public int NumberOfYears { get; set; } = 30;

        [Display(Name = "Periods per Year")]
        public int PeriodsPerYear { get; set; } = 12;

        [Display(Name = "Annual Interest Rate")]
        public decimal AnnualInterestRate { get; set; }

        [Display(Name = "Payment Modifications")]
        public List<AmortizationInputPaymentModificationViewModel> PaymentModifications { get; set; } = [];
    }

    private sealed class AmortizationInputPaymentModificationViewModel
    {
        [Display(Name = "Period Number")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} must be at least 1.")]
        public int PeriodNumber { get; set; }

        [Display(Name = "Modification Amount")]
        public decimal ModificationAmount { get; set; }
    }

    private sealed class AmortizationOutputViewModel
    {
        public string TotalPrincipal { get; set; } = String.Empty;
        public string PaymentPerPeriod { get; set; } = String.Empty;
        public string TotalInterestPaid { get; set; } = String.Empty;
        public string TotalEscrowPaid { get; set; } = String.Empty;
        public string TotalPaid { get; set; } = String.Empty;
        public string InterestSaved { get; set; } = string.Empty;
        public string PeriodsSaved { get; set; } = string.Empty;
        public IReadOnlyList<AmortizationOutputPeriodViewModel> Schedule { get; set; } = new List<AmortizationOutputPeriodViewModel>();
    }

    private sealed class AmortizationOutputPeriodViewModel
    {
        public AmortizationOutputPeriodViewModel(int periodNumber, decimal interestPayment, decimal principalPayment, decimal balanceLeft)
        {
            PeriodNumber = periodNumber.ToString();
            InterestPayment = $"{interestPayment:c}";
            PrincipalPayment = $"{principalPayment:c}";
            BalanceLeft = $"{balanceLeft:c}";
        }

        public string PeriodNumber { get; }
        public string InterestPayment { get; }
        public string PrincipalPayment { get; }
        public string BalanceLeft { get; }
    }
}
